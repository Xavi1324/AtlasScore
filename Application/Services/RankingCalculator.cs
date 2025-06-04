using Application.Dtos.Simulador;
using Persistence.Entities;

namespace Application.Services
{
    public class RankingCalculator
    {
        public List<SimulacionResultadoDto> CalcularRanking(
            IEnumerable<IndicadorPorPais> indicadores,
            IEnumerable<PesoMacroindicadorSimuladoDto> pesosSimulados,
            IEnumerable<Macroindicador> macroindicadores,
            IEnumerable<Pais> paises,
            decimal tasaMinima,
            decimal tasaMaxima)
        {
            var pesoTotal = pesosSimulados.Sum(p => p.Peso);
            if (Math.Abs(pesoTotal - 1m) > 0.0001m)
                throw new ArgumentException("La suma de los pesos debe ser exactamente 1.");

            var minMaxPorMacro = indicadores
                .GroupBy(i => i.MacroindicadorId)
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        Min = g.Min(x => x.Valor),
                        Max = g.Max(x => x.Valor)
                    });

            var sentidoPorMacro = macroindicadores
                .ToDictionary(m => m.Id, m => m.EsMejorMasAlto);

            var resultado = new List<SimulacionResultadoDto>();

            foreach (var pais in paises)
            {
                var indicadoresDelPais = indicadores.Where(i => i.PaisId == pais.Id).ToList();
                if (!indicadoresDelPais.Any()) continue;

                decimal scoringTotal = 0m;

                foreach (var peso in pesosSimulados)
                {
                    var indicador = indicadoresDelPais
                        .FirstOrDefault(i => i.MacroindicadorId == peso.MacroindicadorId);
                    if (indicador == null) continue;

                    var rango = minMaxPorMacro[peso.MacroindicadorId];
                    if (rango.Max == rango.Min)
                    {
                        scoringTotal += 0.5m * peso.Peso;
                        continue;
                    }

                    var normalizado = (indicador.Valor - rango.Min) / (rango.Max - rango.Min);

                    if (!sentidoPorMacro[peso.MacroindicadorId])
                        normalizado = 1 - normalizado;

                    scoringTotal += normalizado * peso.Peso;
                }

                var tasa = tasaMinima + (tasaMaxima - tasaMinima) * scoringTotal;

                resultado.Add(new SimulacionResultadoDto
                {
                    Pais = pais.Nombre,
                    Scoring = Math.Round(scoringTotal, 4),
                    TasaEstimacion = Math.Round(tasa, 4)
                });
            }

            return resultado
                .OrderByDescending(r => r.Scoring)
                .ToList();
        }
    }
}
