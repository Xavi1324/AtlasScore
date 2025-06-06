using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Simulador
{
    public class SimulacionResultadoViewModel
    {
        public int PaisId { get; set; }
        public string Pais { get; set; } = string.Empty;
        public string CodigoIso { get; set; } = string.Empty;
        public decimal Scoring { get; set; }
        public decimal TasaEstimacion { get; set; }
    }
}
