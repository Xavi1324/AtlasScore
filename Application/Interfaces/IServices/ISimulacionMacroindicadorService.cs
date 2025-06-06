using Application.Dtos.Simulador;
using Application.Interfaces.IServices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface ISimulacionMacroindicadorService : IBaseServices<PesoMacroindicadorSimuladoDto>, IPesoValidacionService
    {
        Task<bool> ExisteMacroindicadorAsync(int macroId, int? excluirId = null);
        Task<List<MacroindicadorDisponibleDto>> GetMacroindicadoresDisponiblesAsync();
        Task<List<AñoDisponibleDto>> GetAñosDisponiblesAsync();

        // ← FIRMA CORRECTA: devuelve SimulacionResultadoDto, NO AñoDisponibleDto
        Task<(List<SimulacionResultadoDto> Resultado, string MensajeError)> GenerarRankingAsync(int año);
    }
}
