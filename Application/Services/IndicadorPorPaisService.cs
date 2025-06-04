using Application.Dtos.IndicadorPorPais;
using Application.Interfaces.IServices;
using Persistence.Entities;
using Persistence.Interfaces.IRepositories;

namespace Application.Services
{
    public class IndicadorPorPaisService : IIndicadorPorPaisService
    {
        private readonly IGenericRepository<IndicadorPorPais> _repository;

        public IndicadorPorPaisService(IGenericRepository<IndicadorPorPais> repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(IndicadorPorPaisDto dto)
        {
            var  entity = new IndicadorPorPais
            {
                PaisId = dto.PaisId,
                MacroindicadorId = dto.MacroindicadorId,
                Año = dto.Año,
                Valor = dto.Valor
            };
            await _repository.AddAsync(entity);
            
        }

        public async Task DeleteAsync(int id)
        {
           var entity = await _repository.GetByIdAsync(id);
            if (entity is not null)
                await _repository.DeleteAsync(entity);

        }

        public async Task<List<IndicadorPorPaisDto>> GetAllAsync()
        {
            var enities = await _repository.GetAllAsync();

            return enities.Select(e => new IndicadorPorPaisDto
            {
                Id = e.Id,
                PaisId = e.PaisId,
                MacroindicadorId = e.MacroindicadorId,
                Año = e.Año,
                Valor = e.Valor,
                NombrePais = e.Pais?.Nombre ?? string.Empty,
                NombreMacroindicador = e.Macroindicador?.Nombre ?? string.Empty
            }).ToList();
        }

        public async Task<IndicadorPorPaisDto?> GetByIdAsync(int id)
        {
            var e = await _repository.GetByIdAsync(id);
            if (e == null) return null;

            return new IndicadorPorPaisDto
            {
                Id = e.Id,
                PaisId = e.PaisId,
                MacroindicadorId = e.MacroindicadorId,
                Año = e.Año,
                Valor = e.Valor,
                NombrePais = e.Pais?.Nombre ?? string.Empty,
                NombreMacroindicador = e.Macroindicador?.Nombre ?? string.Empty
            };
        }

        public async Task UpdateAsync(IndicadorPorPaisDto dto)
        {
            var entity = await _repository.GetByIdAsync(dto.Id);
            if (entity == null) return;

            entity.PaisId = dto.PaisId;
            entity.MacroindicadorId = dto.MacroindicadorId;
            entity.Año = dto.Año;
            entity.Valor = dto.Valor;

            await _repository.EditAsync(entity);
        }
        public async Task<bool> ExisteDuplicadoAsync(int paisId, int macroId, int año, int? excluirId = null)
        {
            var lista = await _repository.GetAllAsync();
            return lista.Any(x =>
                x.PaisId == paisId &&
                x.MacroindicadorId == macroId &&
                x.Año == año &&
                (!excluirId.HasValue || x.Id != excluirId));
        }
        public async Task<List<IndicadorPorPaisDto>> FiltrarAsync(int? paisId, int? año)
        {
            var todos = await _repository.GetAllAsync();

            var filtrado = todos.AsQueryable();

            if (paisId.HasValue)
                filtrado = filtrado.Where(x => x.PaisId == paisId.Value);

            if (año.HasValue)
                filtrado = filtrado.Where(x => x.Año == año.Value);

            return filtrado
                .Select(x => new IndicadorPorPaisDto
                {
                    Id = x.Id,
                    PaisId = x.PaisId,
                    MacroindicadorId = x.MacroindicadorId,
                    Año = x.Año,
                    Valor = x.Valor
                }).ToList();
        }

    }

}

