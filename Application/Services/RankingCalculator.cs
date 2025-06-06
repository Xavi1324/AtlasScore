// Application/Services/RankingCalculator.cs
using Application.Dtos.Simulador;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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
            // 1) Validar suma de pesos = 1
            var pesoTotal = pesosSimulados.Sum(p => p.Peso);
            if (Math.Abs(pesoTotal - 1m) > 0.0001m)
                throw new ArgumentException("La suma de los pesos debe ser exactamente 1.");

            // 2) Calcular min y max para cada macro (solo dentro de indicadores filtrados por año y macros)
            var minMaxPorMacro = indicadores
                .GroupBy(i => i.MacroindicadorId)
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        Min = g.Min(x => x.Valor),
                        Max = g.Max(x => x.Valor)
                    });

            // 3) Estado "es mejor más alto" para cada macro
            var sentidoPorMacro = macroindicadores
                .ToDictionary(m => m.Id, m => m.EsMejorMasAlto);

            var resultado = new List<SimulacionResultadoDto>();

            // 4) Para cada país elegible (se asume que la lista `paises` ya está filtrada fuera de aquí)
            foreach (var pais in paises)
            {
                var indicadoresDelPais = indicadores.Where(i => i.PaisId == pais.Id).ToList();
                if (!indicadoresDelPais.Any()) continue;

                decimal scoringTotal = 0m;

                // 5) Para cada peso (macro con peso > 0, porque el servicio solo le pasará esos)
                foreach (var peso in pesosSimulados.Where(p => p.Peso > 0m))
                {
                    var indicador = indicadoresDelPais
                        .FirstOrDefault(i => i.MacroindicadorId == peso.MacroindicadorId);
                    if (indicador == null)
                    {
                        // Si falta dato, se omite; pero en teoría el servicio ya filtró los países que no cumplan todos.
                        continue;
                    }

                    var rango = minMaxPorMacro[peso.MacroindicadorId];
                    if (rango.Max == rango.Min)
                    {
                        // todos los países tienen el mismo valor para este macro: puntaje 0.5 fijo
                        scoringTotal += 0.5m * peso.Peso;
                    }
                    else
                    {
                        var normalizado = (indicador.Valor - rango.Min) / (rango.Max - rango.Min);
                        if (!sentidoPorMacro[peso.MacroindicadorId])
                            normalizado = 1 - normalizado;

                        scoringTotal += normalizado * peso.Peso;
                    }
                }

                // 6) Calcular tasa
                var tasa = tasaMinima + (tasaMaxima - tasaMinima) * scoringTotal;

                // 7) Agregar al resultado
                resultado.Add(new SimulacionResultadoDto
                {
                    PaisId = pais.Id,
                    Pais = pais.Nombre,
                    Scoring = Math.Round(scoringTotal, 4),
                    TasaEstimacion = Math.Round(tasa, 4)
                });
            }

            // 8) Ordenación descendente
            return resultado
                .OrderByDescending(r => r.Scoring)
                .ToList();
        }
    }
}
