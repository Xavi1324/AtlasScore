using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Simulador
{
    public class PesoMacroindicadorSimuladoViewModel
    {
        public int Id { get; set; }
        public int MacroindicadorId { get; set; }
        public string Nombre { get; set; } = string.Empty;

        [Range(0, 1, ErrorMessage = "El peso debe estar entre 0 y 1.")]
        public decimal Peso { get; set; }
        public bool EsMejorMasAlto { get; set; }
    }
}
