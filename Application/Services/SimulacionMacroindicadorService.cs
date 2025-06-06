// Application/Services/SimulacionMacroindicadorService.cs

using Application.Dtos.Simulador;
using Application.Interfaces.IServices;
using Persistence.Entities;
using Persistence.Interfaces.IRepositories;

namespace Application.Services
{
    public class SimulacionMacroindicadorService : ISimulacionMacroindicadorService
    {
        private readonly IGenericRepository<SimulacionMacroindicador> _repoSimulacion;
        private readonly IGenericRepository<Macroindicador> _repoMacroindicador;
        private readonly IGenericRepository<IndicadorPorPais> _repoIndicadorPorPais;
        private readonly IGenericRepository<Pais> _repoPais;
        private readonly IGenericRepository<TasaRetorno> _repoTasa;
        private readonly RankingCalculator _rankingCalculator;

        public SimulacionMacroindicadorService(
            IGenericRepository<SimulacionMacroindicador> repoSimulacion,
            IGenericRepository<Macroindicador> repoMacroindicador,
            IGenericRepository<IndicadorPorPais> repoIndicadorPorPais,
            IGenericRepository<Pais> repoPais,
            RankingCalculator rankingCalculator,
            IGenericRepository<TasaRetorno> repoTasa)
        {
            _repoSimulacion       = repoSimulacion;
            _repoMacroindicador   = repoMacroindicador;
            _repoIndicadorPorPais = repoIndicadorPorPais;
            _repoPais             = repoPais;
            _rankingCalculator    = rankingCalculator;
            _repoTasa             = repoTasa;
        }

        #region ───── CRUD (IBaseServices<PesoMacroindicadorSimuladoDto>) ─────

        public async Task CreateAsync(PesoMacroindicadorSimuladoDto dto)
        {
            if (await ExisteMacroindicadorAsync(dto.MacroindicadorId))
                throw new InvalidOperationException("El macroindicador ya está agregado en la simulación.");

            if (!await PuedeAgregarConPesoAsync(dto.Peso))
                throw new InvalidOperationException("La suma de los pesos excedería 1.");

            var entidad = new SimulacionMacroindicador
            {
                MacroindicadorId = dto.MacroindicadorId,
                PesoSimulacion   = dto.Peso
            };
            await _repoSimulacion.AddAsync(entidad);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repoSimulacion.GetByIdAsync(id);
            if (entity != null)
                await _repoSimulacion.DeleteAsync(entity);
        }

        public async Task<List<PesoMacroindicadorSimuladoDto>> GetAllAsync()
        {
            var listaSim   = await _repoSimulacion.GetAllAsync();
            var listaMacro = await _repoMacroindicador.GetAllAsync();

            return listaSim.Select(e =>
            {
                var macro = listaMacro.FirstOrDefault(m => m.Id == e.MacroindicadorId);
                return new PesoMacroindicadorSimuladoDto
                {
                    Id               = e.Id,
                    MacroindicadorId = e.MacroindicadorId,
                    Nombre           = macro?.Nombre ?? string.Empty,
                    Peso             = e.PesoSimulacion,
                    EsMejorMasAlto   = macro?.EsMejorMasAlto ?? true
                };
            }).ToList();
        }

        public async Task<PesoMacroindicadorSimuladoDto?> GetByIdAsync(int id)
        {
            var sim = await _repoSimulacion.GetByIdAsync(id);
            if (sim == null) return null;

            var macro = await _repoMacroindicador.GetByIdAsync(sim.MacroindicadorId);
            return new PesoMacroindicadorSimuladoDto
            {
                Id               = sim.Id,
                MacroindicadorId = sim.MacroindicadorId,
                Nombre           = macro?.Nombre ?? string.Empty,
                Peso             = sim.PesoSimulacion,
                EsMejorMasAlto   = macro?.EsMejorMasAlto ?? true
            };
        }

        public async Task UpdateAsync(PesoMacroindicadorSimuladoDto dto)
        {
            var entity = await _repoSimulacion.GetByIdAsync(dto.Id);
            if (entity == null) return;

            if (!await PuedeActualizarPesoAsync(dto.Id, dto.Peso))
                throw new InvalidOperationException("La suma de los pesos excedería 1.");

            entity.PesoSimulacion = dto.Peso;
            await _repoSimulacion.EditAsync(entity);
        }

        #endregion

        #region ───── Validación de Pesos (IPesoValidacionService) ─────

        public async Task<decimal> ObtenerSumaDePesosAsync()
        {
            var lista = await _repoSimulacion.GetAllAsync();
            return lista.Sum(x => x.PesoSimulacion);
        }

        public async Task<bool> PuedeAgregarConPesoAsync(decimal nuevoPeso)
        {
            var suma = await ObtenerSumaDePesosAsync();
            return (suma + nuevoPeso) <= 1m;
        }

        public async Task<bool> PuedeActualizarPesoAsync(int id, decimal nuevoPeso)
        {
            var lista = await _repoSimulacion.GetAllAsync();
            var actual = lista.FirstOrDefault(x => x.Id == id);
            if (actual == null) return false;

            var sumaSinActual = lista.Where(x => x.Id != id).Sum(x => x.PesoSimulacion);
            return (sumaSinActual + nuevoPeso) <= 1m;
        }

        #endregion

        #region ───── Métodos de Simulación ─────

        public async Task<bool> ExisteMacroindicadorAsync(int macroId, int? excluirId = null)
        {
            var lista = await _repoSimulacion.GetAllAsync();
            return lista.Any(x =>
                x.MacroindicadorId == macroId &&
                (!excluirId.HasValue || x.Id != excluirId.Value));
        }

        public async Task<List<MacroindicadorDisponibleDto>> GetMacroindicadoresDisponiblesAsync()
        {
            var listaMacro   = await _repoMacroindicador.GetAllAsync();
            var simuladosIds = (await _repoSimulacion.GetAllAsync()).Select(s => s.MacroindicadorId).ToList();

            return listaMacro
                .Where(m => !simuladosIds.Contains(m.Id))
                .Select(m => new MacroindicadorDisponibleDto
                {
                    Id     = m.Id,
                    Nombre = m.Nombre
                })
                .ToList();
        }

        public async Task<List<AñoDisponibleDto>> GetAñosDisponiblesAsync()
        {
            var todosIndicadores = await _repoIndicadorPorPais.GetAllAsync();
            return todosIndicadores
                .Select(i => i.Año)
                .Distinct()
                .OrderByDescending(a => a)
                .Select(a => new AñoDisponibleDto { Año = a })
                .ToList();
        }

        public async Task<(List<SimulacionResultadoDto> Resultado, string MensajeError)> GenerarRankingAsync(int año)
        {
            // 1) Obtener los pesos simulados y validar suma == 1
            var pesosSimulados = await GetAllAsync();
            var sumaPesos = await ObtenerSumaDePesosAsync();
            if (Math.Abs(sumaPesos - 1m) > 0.0001m)
                return (null, "Se deben ajustar los pesos de los macroindicadores agregados a la simulación hasta que la suma de los mismos sea igual a 1");

            // 2) MacroId con peso > 0
            var macroSimulados = pesosSimulados
                .Where(p => p.Peso > 0m)
                .Select(p => p.MacroindicadorId)
                .ToList();

            // 3) Traer todos los países
            var todosPaises = await _repoPais.GetAllAsync();

            // 4) Filtrar IndicadorPorPais por año y por macroSimulados
            var indicadoresFiltrados = (await _repoIndicadorPorPais.GetAllAsync())
                .Where(i => i.Año == año && macroSimulados.Contains(i.MacroindicadorId))
                .ToList();

            // 5) Países elegibles: agrupar por PaisId, contar macroId distintos
            var paisesElegiblesIds = indicadoresFiltrados
                .GroupBy(i => i.PaisId)
                .Where(grupo => grupo.Select(i => i.MacroindicadorId).Distinct().Count() == macroSimulados.Count)
                .Select(grupo => grupo.Key)
                .ToList();

            var paisesElegibles = todosPaises
                .Where(p => paisesElegiblesIds.Contains(p.Id))
                .ToList();

            // 6) Si <2, devolver mensaje de error
            if (paisesElegibles.Count < 2)
            {
                if (paisesElegibles.Count == 1)
                {
                    var unico = paisesElegibles.First();
                    return (null,
                        $"No hay suficientes países para calcular el ranking y la tasa de retorno; " +
                        $"el único país que cumple con los requisitos es {unico.Nombre}. " +
                        "Debe agregar más indicadores para los demás países en el año seleccionado.");
                }
                return (null, "No hay países que cumplan con los requisitos para la simulación.");
            }

            // 7) Obtener todos los macroindicadores (para EsMejorMasAlto)
            var listaMacroCompleta = await _repoMacroindicador.GetAllAsync();

            
            var tasa = (await _repoTasa.GetAllAsync()).FirstOrDefault();
            decimal tasaMinima = (tasa?.TasaMinima > 0) ? tasa.TasaMinima : 0.02m;
            decimal tasaMaxima = (tasa?.TasaMaxima > 0) ? tasa.TasaMaxima : 0.15m;

            var resultado = _rankingCalculator.CalcularRanking(
                indicadores: indicadoresFiltrados,
                pesosSimulados: pesosSimulados,
                macroindicadores: listaMacroCompleta,
                paises: paisesElegibles,
                tasaMinima: tasaMinima,
                tasaMaxima: tasaMaxima
            );

            // 9) Inyectar Código ISO a cada SimulacionResultadoDto
            foreach (var item in resultado)
            {
                var paisObj = todosPaises.FirstOrDefault(p => p.Id == item.PaisId);
                item.CodigoIso = paisObj?.CodigoIso ?? string.Empty;
            }

            // 10) Devolver la lista ordenada
            return (resultado, null);
        }

        #endregion
    }
}
