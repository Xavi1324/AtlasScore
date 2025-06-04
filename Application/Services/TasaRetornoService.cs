using Application.Dtos.TasaRetorno;
using Application.Interfaces.IServices;
using Persistence.Entities;
using Persistence.Interfaces.IRepositories;

namespace Application.Services
{
    public class TasaRetornoService : ITasaRetornoService
    {
        private readonly IGenericRepository<TasaRetorno> _repository;

        public TasaRetornoService(IGenericRepository<TasaRetorno> repository)
        {
            _repository = repository;
        }
        public async Task<TasaRetorno?> GetAsync()
        {
            var entity = (await _repository.GetAllAsync()).FirstOrDefault();
            return entity;
        }

        public async Task CreateOrUpdateAsync(TasaRetornoDto dto)
        {
            var existente = (await _repository.GetAllAsync()).FirstOrDefault();
            if (existente != null)
            {
                existente.TasaMinima = dto.TasaMinima;
                existente.TasaMaxima = dto.TasaMaxima;
                await _repository.EditAsync(existente);
                return;
            }

            var entity = new TasaRetorno
            {
                TasaMinima = dto.TasaMinima,
                TasaMaxima = dto.TasaMaxima
            };
            await _repository.AddAsync(entity);
        }
    }
}
