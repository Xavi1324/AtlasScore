using Application.Dtos.Pais;
using Application.Interfaces.IServices;
using Persistence.Entities;
using Persistence.Interfaces.IRepositories;

namespace Application.Services
{
    public class PaisService : IPaisService
    {
        private readonly IGenericRepository<Pais> _repository;
        
        public PaisService(IGenericRepository<Pais> repository)
        {
            _repository = repository;
        }
        public async Task CreateAsync(PaisDto dto)
        {
            var entity = new Pais
            {
                Nombre = dto.Nombre,
                CodigoIso = dto.CodigoIso
            };

            await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
                await _repository.DeleteAsync(entity);
        }

        public async Task<List<PaisDto>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return list.Select(p => new PaisDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                CodigoIso = p.CodigoIso
            }).ToList();
        }

        public async Task<PaisDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            return new PaisDto
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                CodigoIso = entity.CodigoIso
            };
        }

        public async Task UpdateAsync(PaisDto dto)
        {
            var entity = new Pais
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                CodigoIso = dto.CodigoIso
            };

            await _repository.EditAsync(entity);
        }
    }
}
