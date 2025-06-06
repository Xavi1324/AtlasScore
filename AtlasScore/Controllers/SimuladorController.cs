using Application.Dtos.Simulador;
using Application.Interfaces.IServices;
using Application.Services;
using Application.ViewModels.Simulador;
using Microsoft.AspNetCore.Mvc;

namespace AtlasScore.Controllers
{
    public class SimuladorController : Controller
    {
        private readonly ISimulacionMacroindicadorService _service;

        public SimuladorController(ISimulacionMacroindicadorService service)
        {
            _service = service;

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1. Levantar todos los pesos simulados actuales
            var macroDtos = await _service.GetAllAsync();
            // 2. Levantar lista de años disponibles
            var años = await _service.GetAñosDisponiblesAsync();
            // 3. Calcular suma de pesos actuales
            var sumaPesos = await _service.ObtenerSumaDePesosAsync();

            // 4. Mapear a ViewModel
            var viewModel = new SimulacionIndexViewModel
            {
                Macroindicadores = macroDtos.Select(m => new PesoMacroindicadorSimuladoViewModel
                {
                    Id = m.Id,
                    MacroindicadorId = m.MacroindicadorId,
                    Nombre = m.Nombre,
                    Peso = m.Peso,
                    EsMejorMasAlto = m.EsMejorMasAlto
                }).ToList(),

                AñosDisponibles = años.Select(a => new AñoDisponibleViewModel { Año = a.Año }).ToList(),

                // Selecciono por defecto el primero (más reciente)
                AñoSeleccionado = años.Any() ? años.First().Año : 0,

                SumaPesos = sumaPesos
            };

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // 1. Validar que la suma de pesos actual sea < 1
            var sumaPesos = await _service.ObtenerSumaDePesosAsync();
            if (sumaPesos >= 1m)
            {
                TempData["Error"] = "No se pueden agregar más macroindicadores porque la suma de pesos ya es 1.";
                return RedirectToAction(nameof(Index));
            }

            // 2. Obtener macros disponibles (no agregados aún)
            var macrosDisp = await _service.GetMacroindicadoresDisponiblesAsync();
            if (!macrosDisp.Any())
            {
                TempData["Error"] = "No hay macroindicadores disponibles para agregar.";
                return RedirectToAction(nameof(Index));
            }

            // 3. Preparar ViewModel
            var viewModel = new CrearMacroindicadorSimulacionViewModel
            {
                MacroindicadoresDisponibles = macrosDisp
                    .Select(m => new MacroindicadorDisponibleViewModel { Id = m.Id, Nombre = m.Nombre })
                    .ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CrearMacroindicadorSimulacionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // Si falla validación del ViewModel, recargar dropdown
                vm.MacroindicadoresDisponibles = (await _service.GetMacroindicadoresDisponiblesAsync())
                    .Select(m => new MacroindicadorDisponibleViewModel { Id = m.Id, Nombre = m.Nombre })
                    .ToList();
                return View(vm);
            }

            try
            {
                // Crear en base de datos
                await _service.CreateAsync(new PesoMacroindicadorSimuladoDto
                {
                    MacroindicadorId = vm.MacroindicadorId,
                    Peso = vm.Peso
                });
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                vm.MacroindicadoresDisponibles = (await _service.GetMacroindicadoresDisponiblesAsync())
                    .Select(m => new MacroindicadorDisponibleViewModel { Id = m.Id, Nombre = m.Nombre })
                    .ToList();
                return View(vm);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return RedirectToAction(nameof(Index));

            var viewModel = new EditarMacroindicadorSimulacionViewModel
            {
                Id = dto.Id,
                Peso = dto.Peso,
                NombreMacroindicador = dto.Nombre
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditarMacroindicadorSimulacionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // Recuperar nombre nuevamente (para mostrarlo si el modelo no es válido)
                var dto = await _service.GetByIdAsync(vm.Id);
                vm.NombreMacroindicador = dto?.Nombre ?? string.Empty;
                return View(vm);
            }

            try
            {
                // Llamada al servicio para actualizar
                await _service.UpdateAsync(new PesoMacroindicadorSimuladoDto
                {
                    Id = vm.Id,
                    // Sacar MacroindicadorId de la base
                    MacroindicadorId = (await _service.GetByIdAsync(vm.Id))?.MacroindicadorId ?? 0,
                    Peso = vm.Peso
                });
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var dto = await _service.GetByIdAsync(vm.Id);
                vm.NombreMacroindicador = dto?.Nombre ?? string.Empty;
                return View(vm);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return RedirectToAction(nameof(Index));

            var viewModel = new EliminarMacroindicadorSimulacionViewModel
            {
                Id = dto.Id,
                NombreMacroindicador = dto.Nombre
            };

            return View(viewModel);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> SimularRanking(SimulacionIndexViewModel vm)
        {
            // 1) Llamar al servicio con el año recibido
            var (resultado, mensajeError) = await _service.GenerarRankingAsync(vm.AñoSeleccionado);

            // 2) Reconstruir el ViewModel para volver a la vista Index
            var macroDtos = await _service.GetAllAsync();
            var años = await _service.GetAñosDisponiblesAsync();
            var sumaPesos = await _service.ObtenerSumaDePesosAsync();
            int año = vm.AñoSeleccionado;

            var viewModel = new SimulacionIndexViewModel
            {
                Macroindicadores = macroDtos.Select(m => new PesoMacroindicadorSimuladoViewModel
                {
                    Id = m.Id,
                    MacroindicadorId = m.MacroindicadorId,
                    Nombre = m.Nombre,
                    Peso = m.Peso,
                    EsMejorMasAlto = m.EsMejorMasAlto
                }).ToList(),

                AñosDisponibles = años.Select(a => new AñoDisponibleViewModel
                {
                    Año = a.Año
                }).ToList(),

                // Aquí guardamos el año que llegó
                AñoSeleccionado = año,
                SumaPesos = sumaPesos,
                MensajeError = mensajeError,

                ResultadoRanking = resultado?
                    .Select(r => new SimulacionResultadoViewModel
                    {
                        Pais = r.Pais,
                        CodigoIso = r.CodigoIso,
                        Scoring = r.Scoring,
                        TasaEstimacion = r.TasaEstimacion
                    })
                    .ToList()
            };

            return View("Index", viewModel);
        }







    }
}
