using Application.Interfaces.IServices;
using Application.ViewModels.IndicadorPorPais;
using Microsoft.AspNetCore.Mvc;

namespace AtlasScore.Controllers
{
    public class IndicadorPorPaisController : Controller
    {
        private readonly IIndicadorPorPaisService _service;
        private readonly IPaisService _paisService;
        private readonly IMacroindicadorService _macroindicadorService;

        public IndicadorPorPaisController(IIndicadorPorPaisService service, IPaisService paisService, IMacroindicadorService macroindicadorService)
        {
            _service = service;
            _paisService = paisService;
            _macroindicadorService = macroindicadorService;
        }

        public async Task<IActionResult> Index(int? paisID, int? año)
        {
            var paises = await _paisService.GetAllAsync();
            var macroindicadores = await _macroindicadorService.GetAllAsync();
            var dtoList = await _service.FiltrarAsync(paisID, año);

            var viewModelList = dtoList.Select(x => new IndicadorPorPaisViewModel
            {
                Id = x.Id,
                PaisId = x.PaisId,
                MacroindicadorId = x.MacroindicadorId,
                Año = x.Año,
                Valor = x.Valor,
                NombrePais = paises.FirstOrDefault(p => p.Id == x.PaisId)?.Nombre ?? "",
                NombreMacroindicador = macroindicadores.FirstOrDefault(m => m.Id == x.MacroindicadorId)?.Nombre ?? ""
            }).ToList();

            ViewBag.Paises = paises;
            ViewBag.SelectedPaisId = paisID ?? 0;
            ViewBag.SelectedAño = año;

            return View(viewModelList);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Paises = await _paisService.GetAllAsync();
            ViewBag.Macroindicadores = await _macroindicadorService.GetAllAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(IndicadorPorPaisViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Paises = await _paisService.GetAllAsync();
                ViewBag.Macroindicadores = await _macroindicadorService.GetAllAsync();
                return View(vm);
            }

            var duplicado = await _service.ExisteDuplicadoAsync(vm.MacroindicadorId, vm.Año);

            if (duplicado)
            {
                ModelState.AddModelError(string.Empty, "Ya existe un indicador para el macroindicador y año seleccionados.");
                ViewBag.Paises = await _paisService.GetAllAsync();
                ViewBag.Macroindicadores = await _macroindicadorService.GetAllAsync();
                return View();
            }

            var dto = new IndicadorPorPaisDto
            {

                PaisId = vm.PaisId,
                MacroindicadorId = vm.MacroindicadorId,
                Año = vm.Año,
                Valor = vm.Valor
            };
            await _service.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();

            ViewBag.Paises = await _paisService.GetAllAsync();
            ViewBag.Macroindicadores = await _macroindicadorService.GetAllAsync();

            var vm = new IndicadorPorPaisViewModel
            {
                Id = dto.Id,
                PaisId = dto.PaisId,
                MacroindicadorId = dto.MacroindicadorId,
                Año = dto.Año,
                Valor = dto.Valor
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, IndicadorPorPaisViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Paises = await _paisService.GetAllAsync();
                ViewBag.Macroindicadores = await _macroindicadorService.GetAllAsync();
                return View(vm);
            }
            var duplicado = await _service.ExisteDuplicadoAsync(vm.MacroindicadorId, vm.Año, id);

            if (duplicado)
            {
                ModelState.AddModelError(string.Empty, "Ya existe un indicador para el macroindicador y año seleccionados.");
                ViewBag.Paises = await _paisService.GetAllAsync();
                ViewBag.Macroindicadores = await _macroindicadorService.GetAllAsync();
                return View(vm);
            }
            var dto = new IndicadorPorPaisDto
            {
                Id = vm.Id,
                PaisId = vm.PaisId,
                MacroindicadorId = vm.MacroindicadorId,
                Año = vm.Año,
                Valor = vm.Valor
            };
            await _service.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var pais = await _paisService.GetByIdAsync(dto.PaisId);
            var macroindicador = await _macroindicadorService.GetByIdAsync(dto.MacroindicadorId);

            var vm = new IndicadorPorPaisViewModel
            {
                Id = dto.Id,
                PaisId = dto.PaisId,
                MacroindicadorId = dto.MacroindicadorId,
                Año = dto.Año,
                Valor = dto.Valor,
                NombrePais = pais?.Nombre ?? "",
                NombreMacroindicador = macroindicador?.Nombre ?? ""

            };
            return View(vm);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
