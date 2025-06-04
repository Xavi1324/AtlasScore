using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Simulador
{
    public class SimulacionRequestDto
    {
        public Dictionary<int, decimal> PesosSimulados { get; set; } = new(); // clave: MacroindicadorId
        public decimal TasaMinima { get; set; }
        public decimal TasaMaxima { get; set; }
    }
}
