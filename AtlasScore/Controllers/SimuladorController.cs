using Application.Dtos.Simulador;
using Application.Interfaces.IServices;
using Application.Services;
using Application.ViewModels.Simulador;
using Microsoft.AspNetCore.Mvc;

namespace AtlasScore.Controllers
{
    public class SimuladorController : Controller
    {
        private readonly IMacroindicadorService _macroService;
        private readonly IIndicadorPorPaisService _indicadorService;
        private readonly ITasaRetornoService _tasaService;
        private readonly IPaisService _paisService;

        public SimuladorController(IMacroindicadorService macroService, IIndicadorPorPaisService indicadorService, ITasaRetornoService tasaService, IPaisService paisService)
        {
            _macroService = macroService;
            _indicadorService = indicadorService;
            _tasaService = tasaService;
            _paisService = paisService;
        }
        [HttpGet]
        public async Task<IActionResult> Simulacion()
        {
            var macros = await _macroService.GetAllAsync();
            var tasa = await _tasaService.GetAsync();

            var viewModel = new SimuladorViewModel
            {
                PesosSimulados = macros.Select(m => new PesoMacroindicadorSimuladoViewModel
                {
                    MacroindicadorId = m.Id,
                    Nombre = m.Nombre,
                    Peso = 0, // por defecto
                    EsMejorMasAlto = m.EsMejorMasAlto
                }).ToList(),
                TasaMinima = tasa?.TasaMinima,
                TasaMaxima = tasa?.TasaMaxima
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Simulacion(SimuladorViewModel vm)
        {
            var sumaPesos = vm.PesosSimulados.Sum(p => p.Peso);
            if (Math.Abs(sumaPesos - 1m) > 0.0001m)
            {
                ModelState.AddModelError(string.Empty, "La suma de los pesos debe ser exactamente 1.");
            }

            if (!ModelState.IsValid)
            {
                var tasa = await _tasaService.GetAsync();
                vm.TasaMinima = tasa?.TasaMinima;
                vm.TasaMaxima = tasa?.TasaMaxima;
                return View(vm);
            }

            var indicadores = await _indicadorService.GetAllAsync();
            var macros = await _macroService.GetAllAsync();
            var paises = await _paisService.GetAllAsync();
            var tasaRetorno = await _tasaService.GetAsync();

            var pesos = vm.PesosSimulados.Select(p => new PesoMacroindicadorSimuladoDto
            {
                MacroindicadorId = p.MacroindicadorId,
                Peso = p.Peso
            }).ToList();

            var calculadora = new RankingCalculator();

            var resultado = calculadora.CalcularRanking(
                indicadores: indicadores.Select(x => new Persistence.Entities.IndicadorPorPais
                {
                    Id = x.Id,
                    PaisId = x.PaisId,
                    MacroindicadorId = x.MacroindicadorId,
                    Año = x.Año,
                    Valor = x.Valor
                }),
                pesosSimulados: pesos,
                macroindicadores: macros.Select(m => new Persistence.Entities.Macroindicador
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Peso = m.Peso,
                    EsMejorMasAlto = m.EsMejorMasAlto
                }),
                paises: paises.Select(p => new Persistence.Entities.Pais
                {
                    Id = p.Id,
                    Nombre = p.Nombre
                }),
                tasaMinima: tasaRetorno?.TasaMinima ?? 0,
                tasaMaxima: tasaRetorno?.TasaMaxima ?? 1
            );

            vm.Resultados = resultado.Select(r => new ResultadoSimuladoViewModel
            {
                Pais = r.Pais,
                Scoring = r.Scoring,
                TasaEstimacion = r.TasaEstimacion
            }).ToList();

            return View(vm);
        }
    }
}
