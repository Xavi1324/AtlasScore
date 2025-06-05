using Application.Dtos.IndicadorPorPais;
using Application.Interfaces.IServices.Common;

namespace Application.Interfaces.IServices
{
    public interface IIndicadorPorPaisService : IBaseServices<IndicadorPorPaisDto>
    {

        Task<bool> ExisteDuplicadoAsync(int paisId, int macroindicadorId, int año, int? idExcluir = null);
        Task<List<IndicadorPorPaisDto>> FiltrarAsync(int? paisId, int? año);


    }
}
