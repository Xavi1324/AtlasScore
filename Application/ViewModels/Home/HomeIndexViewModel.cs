using Application.Dtos.Simulador;
using Application.ViewModels.Simulador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public int Año { get; set; }
        public List<AñoDisponibleDto> AñosDisponibles { get; set; } = new();
        public List<SimulacionResultadoDto> Resultados { get; set; } = new();
        
    }
}
