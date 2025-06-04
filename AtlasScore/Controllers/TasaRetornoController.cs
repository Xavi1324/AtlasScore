using Application.Dtos.TasaRetorno;
using Application.Interfaces.IServices;
using Application.ViewModels.TasaRetorno;
using Microsoft.AspNetCore.Mvc;

namespace AtlasScore.Controllers
{
    public class TasaRetornoController : Controller
    {
        private readonly ITasaRetornoService _service;

        public TasaRetornoController(ITasaRetornoService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Configuracion()
        {
            var dto = await _service.GetAsync();

            var vm = dto == null ? new TasaRetornoViewModel() : new TasaRetornoViewModel
            {
                Id = dto.Id,
                TasaMinima = dto.TasaMinima,
                TasaMaxima = dto.TasaMaxima
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Configuracion(TasaRetornoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (vm.TasaMinima >= vm.TasaMaxima)
            {
                ModelState.AddModelError(string.Empty, "La tasa mínima debe ser menor que la tasa máxima.");
                return View(vm);
            }

            var entity = new TasaRetornoDto
            {
                Id = vm.Id,
                TasaMinima = vm.TasaMinima,
                TasaMaxima = vm.TasaMaxima
            };
            await _service.CreateOrUpdateAsync(entity);

            TempData["SuccessMessage"] = "Los valores fueron guardados correctamente.";
            return RedirectToAction(nameof(Configuracion));
        }
    }

}
