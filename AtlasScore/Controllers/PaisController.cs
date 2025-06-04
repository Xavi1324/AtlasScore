using Application.ViewModels.Pais;
using Application.Interfaces.IServices;
using Application.ViewModels.Pais;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entities;
using Application.Dtos.Pais;

namespace AtlasScore.Controllers
{
    public class PaisController : Controller
    {
        private readonly IPaisService _paisService;

        public PaisController(IPaisService paisService)
        {
            _paisService = paisService;
        }

        public async Task<IActionResult> PaisIndex()
        {
            var paisesDto = await _paisService.GetAllAsync();
            var viewModels = paisesDto.Select(p => new PaisViewModel
            {
                Id = p.Id,
                Nombre = p.Nombre,
                CodigoIso = p.CodigoIso
            }).ToList();

            return View(viewModels);

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PaisViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new PaisDto
            {
                Nombre = vm.Nombre,
                CodigoIso = vm.CodigoIso
            };
            await _paisService.CreateAsync(dto);
            return RedirectToAction("PaisIndex");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _paisService.GetByIdAsync(id);
            if (entity == null) return NotFound();
            var vm = new PaisViewModel
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                CodigoIso = entity.CodigoIso
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PaisViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = new PaisDto
            {
                Id = vm.Id,
                Nombre = vm.Nombre,
                CodigoIso = vm.CodigoIso
            };
            await _paisService.UpdateAsync(dto);
            return RedirectToAction("PaisIndex");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _paisService.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var vm = new PaisViewModel
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                CodigoIso = entity.CodigoIso
            };
            return View(vm);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _paisService.DeleteAsync(id);
            return RedirectToAction("PaisIndex");
        }
    }
}