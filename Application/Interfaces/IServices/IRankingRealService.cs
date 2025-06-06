using Application.Dtos.Simulador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IRankingRealService
    {
        Task<List<AñoDisponibleDto>> GetAñosDisponiblesAsync();

        Task<(List<SimulacionResultadoDto> Resultado, string MensajeError)> GenerarRankingRealAsync(int año);

        Task<(List<SimulacionResultadoDto> Resultado, string MensajeError)> CalcularRankingRealAsync(int año);
    }
}
