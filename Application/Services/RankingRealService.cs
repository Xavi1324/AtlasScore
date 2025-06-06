using Application.Dtos.Simulador;
using Application.Interfaces.IServices;
using Persistence.Entities;
using Persistence.Interfaces.IRepositories;

namespace Application.Services
{
    public class RankingRealService  : IRankingRealService
    {
        private readonly IGenericRepository<Macroindicador> _macroRepo;
        private readonly IGenericRepository<IndicadorPorPais> _indicadorRepo;
        private readonly IGenericRepository<Pais> _paisRepo;
        private readonly IGenericRepository<TasaRetorno> _tasaRepo;
        private readonly RankingCalculator _rankingCalculator;

        public RankingRealService(IGenericRepository<Macroindicador> macroRepo, IGenericRepository<IndicadorPorPais> indicadorRepo, IGenericRepository<Pais> paisRepo, IGenericRepository<TasaRetorno> tasaRepo, RankingCalculator rankingCalculator)
        {
            _macroRepo = macroRepo;
            _indicadorRepo = indicadorRepo;
            _paisRepo = paisRepo;
            _tasaRepo = tasaRepo;
            _rankingCalculator = rankingCalculator;
        }

        public async Task<(List<SimulacionResultadoDto> Resultado, string MensajeError)> CalcularRankingRealAsync(int año)
        {
            var macros = await _macroRepo.GetAllAsync();
            if (!macros.Any())
            {
                return (null, "No hay macroindicadores definidos");
            }

            var sumaPesos = macros.Sum(m => m.Peso);
            if(Math.Abs(sumaPesos - 1m) > 0.0001m)
            {
                return (null, "La suma de los pesos de los macroindicadores debe ser exactamente 1.");
            }

            var indicadores = (await _indicadorRepo.GetAllAsync()).Where(i => i != null).ToList();
            if (!indicadores.Any())
            {
                return (null, $"No hay indicadores registrados para el año {año}.");
            }
            var macroIds = macros.Select(m => m.Id).ToList();

            var paisesValidosIds = indicadores
                .GroupBy(i =>i.PaisId)
                .Where(grp => grp.Select(i => i.MacroindicadorId)
                .Distinct()
                .Count() == macroIds.Count)
                .Select(grp => grp.Key)
                .ToList();

            if (paisesValidosIds.Count < 2)
            {
                return (null, "No hay suficientes países con todos los macroindicadores en el año seleccionado.");
            }

            var pasises = ( await _paisRepo.GetAllAsync()).Where(p => paisesValidosIds.Contains(p.Id)).ToList();

            var tasa = (await _tasaRepo.GetAllAsync()).FirstOrDefault();
            decimal tasaMin = (tasa?.TasaMinima > 0 ? tasa.TasaMinima : 0.02m);
            decimal tasaMax = (tasa?.TasaMaxima > 0 ? tasa.TasaMaxima : 0.15m);

            var pesos = macros.Select(m => new PesoMacroindicadorSimuladoDto
            {
                MacroindicadorId = m.Id,
                Peso = m.Peso
            });

            var resultado = _rankingCalculator.CalcularRanking(
                indicadores: indicadores,
                pesosSimulados: pesos,
                macroindicadores: macros,
                paises: pasises,
                tasaMinima: tasaMin,
                tasaMaxima: tasaMax
                );

            foreach ( var r in resultado )
            {
                r.CodigoIso = pasises.FirstOrDefault(p => p.Id == r.PaisId)?.CodigoIso ?? "";
            }
            return (resultado, null);
        }

        public async Task<(List<SimulacionResultadoDto> Resultado, string MensajeError)> GenerarRankingRealAsync(int año)
        {
            var macros = await _macroRepo.GetAllAsync();
            var sumaPesos = macros.Sum(m => m.Peso);

            if (Math.Abs(sumaPesos - 1m) > 0.0001m)
            {
                return (null, "Se deben ajustar los pesos de los macroindicadores registrados hasta que la suma de los mismos sea igual a 1.");
            }

            var indicadores = await _indicadorRepo.GetAllAsync();
            var indicadoresFiltrados = indicadores.Where(i => i.Año == año).ToList();

            var macrosConPeso = macros.Where(m => m.Peso > 0).ToList();
            var paises = await _paisRepo.GetAllAsync();

            var paisesElegibles = paises.Where(p =>
            {
                var indicadoresPais = indicadoresFiltrados
                    .Where(i => i.PaisId == p.Id)
                    .Select(i => i.MacroindicadorId)
                    .Distinct()
                    .ToList();

                return macrosConPeso.All(m => indicadoresPais.Contains(m.Id));
            }).ToList();

            if (paisesElegibles.Count < 2)
            {
                if (paisesElegibles.Count == 1)
                    return (null, $"No hay suficiente países para calcular el ranking y la tasa de retorno. El único país que cumple con los requisitos es {paisesElegibles.First().Nombre}. Debe agregar más indicadores.");
                return (null, "No hay países que cumplan con los requisitos para la simulación.");
            }

            var pesosSimulados = macros.Select(m => new PesoMacroindicadorSimuladoDto
            {
                MacroindicadorId = m.Id,
                Peso = m.Peso
            }).ToList();

            var resultado = _rankingCalculator.CalcularRanking(
                indicadores: indicadoresFiltrados,
                pesosSimulados: pesosSimulados,
                macroindicadores: macros,
                paises: paisesElegibles,
                tasaMinima: 0.02m,
                tasaMaxima: 0.15m
            );

            foreach (var r in resultado)
            {
                var pais = paises.FirstOrDefault(p => p.Id == r.PaisId);
                r.CodigoIso = pais?.CodigoIso ?? string.Empty;
            }

            return (resultado, null);
        }

        public async Task<List<AñoDisponibleDto>> GetAñosDisponiblesAsync()
        {
            var indicadores = await _indicadorRepo.GetAllAsync();
            return indicadores
                .Select(i => i.Año)
                .Distinct()
                .OrderByDescending(a => a)
                .Select(a => new AñoDisponibleDto { Año = a })
                .ToList();
        }
    }
}
