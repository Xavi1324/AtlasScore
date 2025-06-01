using Application.ViewModels.IndicadorPorPais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IIndicadorPorPaisService
    {
        Task<List<IndicadorPorPaisDto>> GetAllAsync();
        Task<IndicadorPorPaisDto?> GetByIdAsync(int id);
        Task CreateAsync(IndicadorPorPaisDto dto);
        Task UpdateAsync(IndicadorPorPaisDto dto);
        Task DeleteAsync(int id);
        Task<bool> ExisteDuplicadoAsync(int macroindicadorId, int año, int? idExcluir = null);
        Task<List<IndicadorPorPaisDto>> FiltrarAsync(int? paisId, int? año);


    }
}
