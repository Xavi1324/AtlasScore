using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Simulador
{
    public class SimulacionIndexViewModel
    {
        public List<PesoMacroindicadorSimuladoViewModel> Macroindicadores { get; set; } = new();

        public List<AñoDisponibleViewModel> AñosDisponibles { get; set; } = new();

        public int AñoSeleccionado { get; set; }

        public List<SimulacionResultadoViewModel>? ResultadoRanking { get; set; }

        public string? MensajeError { get; set; }
        public decimal SumaPesos { get; set; }

    }
}
