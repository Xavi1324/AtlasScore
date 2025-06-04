using Application.Dtos.Macroindicador;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Application.ViewModels.Macroindicador;

namespace AtlasScore.Controllers
{
    public class MacroindicadorController : Controller
    {
        private readonly IMacroindicadorService _service;

        public MacroindicadorController(IMacroindicadorService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _service.GetAllAsync();
            var sumaPesos = await _service.ObtenerSumaDePesosAsync();

            ViewBag.CreacionPermitida = sumaPesos < 1;

            var vms = dtos.Select(m => new MacroindicadorViewModel
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Peso = m.Peso,
                EsMejorMasAlto = m.EsMejorMasAlto
            }).ToList();

            return View(vms);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MacroindicadorViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var esValido = await _service.PuedeAgregarConPesoAsync(vm.Peso);
            if (!esValido)
            {
                ModelState.AddModelError(nameof(vm.Peso), "La suma total de pesos no puede superar 1.");
                return View(vm);
            }

            var dto = new MacroindicadorDto
            {
                Nombre = vm.Nombre,
                Peso = vm.Peso,
                EsMejorMasAlto = vm.EsMejorMasAlto
            };

            await _service.CreateAsync(dto);
            TempData["Success"] = "Macroindicador creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = new MacroindicadorViewModel
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Peso = dto.Peso,
                EsMejorMasAlto = dto.EsMejorMasAlto
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MacroindicadorViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var valido = await _service.PuedeActualizarPesoAsync(vm.Id, vm.Peso);
            if (!valido)
            {
                ModelState.AddModelError(nameof(vm.Peso), "La suma total de pesos no puede superar 1.");
                return View(vm);
            }

            var dto = new MacroindicadorDto
            {
                Id = vm.Id,
                Nombre = vm.Nombre,
                Peso = vm.Peso,
                EsMejorMasAlto = vm.EsMejorMasAlto
            };

            await _service.UpdateAsync(dto);
            TempData["Success"] = "Macroindicador actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = new MacroindicadorViewModel
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Peso = dto.Peso,
                EsMejorMasAlto = dto.EsMejorMasAlto
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Macroindicador eliminado  correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
