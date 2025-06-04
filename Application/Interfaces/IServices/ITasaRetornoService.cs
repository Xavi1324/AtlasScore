using Application.Dtos.TasaRetorno;
using Persistence.Entities;

namespace Application.Interfaces.IServices
{
    public interface ITasaRetornoService
    {
        Task<TasaRetorno?> GetAsync();
        Task CreateOrUpdateAsync(TasaRetornoDto dto); 
    }
}
