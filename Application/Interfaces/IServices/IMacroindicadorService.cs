using Application.Dtos.Macroindicador;

namespace Application.Interfaces.IServices
{
    public interface IMacroindicadorService
    {
        Task<List<MacroindicadorDto>> GetAllAsync();
        Task<MacroindicadorDto?> GetByIdAsync(int id);
        Task CreateAsync(MacroindicadorDto dto);
        Task UpdateAsync(MacroindicadorDto dto);
        Task DeleteAsync(int id);
        Task<decimal> ObtenerSumaDePesosAsync();
        Task<bool> PuedeAgregarConPesoAsync(decimal nuevoPeso);
        Task<bool> PuedeActualizarPesoAsync(int id, decimal nuevoPeso);

    }
}
