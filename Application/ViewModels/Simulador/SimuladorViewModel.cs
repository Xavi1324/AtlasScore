using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Simulador
{
    public class SimuladorViewModel
    {
        [Required(ErrorMessage = "Debe asignar al menos un peso.")]
        public List<PesoMacroindicadorSimuladoViewModel> PesosSimulados { get; set; } = new();

        public List<ResultadoSimuladoViewModel>? Resultados { get; set; } // Se llena tras la simulación

        public decimal? TasaMinima { get; set; }  // Para mostrar en la vista
        public decimal? TasaMaxima { get; set; }  // Para mostrar en la vista

    }
}
