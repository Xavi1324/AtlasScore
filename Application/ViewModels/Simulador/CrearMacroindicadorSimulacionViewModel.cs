using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Simulador
{
    public class CrearMacroindicadorSimulacionViewModel
    {
        [Required(ErrorMessage = "Debes seleccionar un macroindicador.")]
        public int MacroindicadorId { get; set; }

        [Required(ErrorMessage = "El peso es requerido.")]
        [Range(0.01, 1, ErrorMessage = "El peso debe estar entre 0.01 y 1.")]
        public decimal Peso { get; set; }

        public List<MacroindicadorDisponibleViewModel> MacroindicadoresDisponibles { get; set; } = new();

    }
}
