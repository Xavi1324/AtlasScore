using Application.Dtos.Simulador;
using Application.Interfaces.IServices;
using Application.ViewModels.Home;
using Application.ViewModels.Simulador;
using Microsoft.AspNetCore.Mvc;

namespace AtlasScore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRankingRealService _rankingService;
        private readonly IMacroindicadorService _macroService;
        private readonly IIndicadorPorPaisService _indicadorService;

        public HomeController(IRankingRealService rankingService, IMacroindicadorService macroService ,IIndicadorPorPaisService indicadorService)
        {
            _rankingService = rankingService;
            _macroService = macroService;
            _indicadorService = indicadorService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var a�os = await _rankingService.GetA�osDisponiblesAsync();
            var a�oMasReciente = a�os.Any() ? a�os.First().A�o : 0;

            var (resultados, mensajeError) = await _rankingService.GenerarRankingRealAsync(a�oMasReciente);

            var viewModel = new HomeIndexViewModel
            {
                A�o = a�oMasReciente,
                A�osDisponibles = a�os,
                Resultados = resultados ?? new(),
            };
            ViewBag.MensajeError = mensajeError;
            if (mensajeError?.Contains("macroindicadores") == true)
                ViewBag.MacroIndicatorLink = Url.Action("Index", "Macroindicador");
            else if (mensajeError?.Contains("indicadores") == true)
                ViewBag.IndicatorLink = Url.Action("Index", "IndicadorPorPais");

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CalcularRanking(HomeIndexViewModel vm)
        {
            var a�os = await _rankingService.GetA�osDisponiblesAsync();
            var (resultados, mensajeError) = await _rankingService.GenerarRankingRealAsync(vm.A�o);

            var viewModel = new HomeIndexViewModel
            {
                A�o = vm.A�o,
                A�osDisponibles = a�os,
                Resultados = resultados ?? new(),
            };
            ViewBag.MensajeError = mensajeError;
            if (mensajeError?.Contains("macroindicadores") == true)
                ViewBag.MacroIndicatorLink = Url.Action("Index", "Macroindicador");
            else if (mensajeError?.Contains("indicadores") == true)
                ViewBag.IndicatorLink = Url.Action("Index", "IndicadorPorPais");

            return View("Index", viewModel);
        }
    }
}

