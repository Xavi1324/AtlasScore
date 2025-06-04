using Application.Dtos.Pais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IPaisService
    {
        Task<List<PaisDto>> GetAllAsync();
        Task<PaisDto?> GetByIdAsync(int id);
        Task CreateAsync(PaisDto dto);
        Task UpdateAsync(PaisDto dto);
        Task DeleteAsync(int id);


    }
}
