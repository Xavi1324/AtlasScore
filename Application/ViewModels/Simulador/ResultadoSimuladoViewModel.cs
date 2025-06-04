using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Simulador
{
    public class ResultadoSimuladoViewModel
    {
        public string Pais { get; set; } = string.Empty;
        public decimal Scoring { get; set; }
        public decimal TasaEstimacion { get; set; }
    }
}
