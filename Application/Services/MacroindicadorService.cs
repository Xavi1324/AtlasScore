using Application.ViewModels.Macroindicador;
using Application.Interfaces.IServices;
using Persistence.Entities;
using Persistence.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MacroindicadorService : IMacroindicadorService
    {
        private readonly IGenericRepository<Macroindicador> _repository;

        public MacroindicadorService(IGenericRepository<Macroindicador> repository)
        {
            _repository = repository;
        }
        public async Task<List<MacroindicadorDto>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return list.Select(m => new MacroindicadorDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Peso = m.Peso,
                EsMejorMasAlto = m.EsMejorMasAlto
            }).ToList();
        }

        public async Task<MacroindicadorDto?> GetByIdAsync(int id)
        {
            var m = await _repository.GetByIdAsync(id);
            if (m == null) return null;

            return new MacroindicadorDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Peso = m.Peso,
                EsMejorMasAlto = m.EsMejorMasAlto
            };
        }

        public async Task CreateAsync(MacroindicadorDto dto)
        {
            var entity = new Macroindicador
            {
                Nombre = dto.Nombre,
                Peso = dto.Peso,
                EsMejorMasAlto = dto.EsMejorMasAlto
            };
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(MacroindicadorDto dto)
        {
            var entity = await _repository.GetByIdAsync(dto.Id);
            if (entity is null) return;

            entity.Nombre = dto.Nombre;
            entity.Peso = dto.Peso;
            entity.EsMejorMasAlto = dto.EsMejorMasAlto;

            await _repository.EditAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is not null)
                await _repository.DeleteAsync(entity);
        }
        // validaciones
        public async Task<decimal> ObtenerSumaDePesosAsync()
        {
            var list = await _repository.GetAllAsync();
            return list.Sum(m => m.Peso);
        }

        public async Task<bool> PuedeAgregarConPesoAsync(decimal nuevoPeso)
        {
            var sumaActual = await ObtenerSumaDePesosAsync();
            return (sumaActual + nuevoPeso) <= 1;
        }
        public async Task<bool> PuedeActualizarPesoAsync(int id, decimal nuevoPeso)
        {
            var todos = await _repository.GetAllAsync();
            var actual = todos.FirstOrDefault(m => m.Id == id);
            if (actual == null) return false;

            var sumaSinEste = todos.Where(m => m.Id != id).Sum(m => m.Peso);
            return (sumaSinEste + nuevoPeso) <= 1;
        }




    }
}

