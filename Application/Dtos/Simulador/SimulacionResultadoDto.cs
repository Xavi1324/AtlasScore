using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Simulador
{
    public class SimulacionResultadoDto
    {
        public string Pais { get; set; } = string.Empty;
        public decimal Scoring { get; set; }
        public decimal TasaEstimacion { get; set; }
    }
}
