using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Macroindicador
{
    public class MacroindicadorViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Range(0, 100)]
        public decimal Peso { get; set; }

        [Display(Name = "¿Es mejor mientras más alto?")]
        public bool EsMejorMasAlto { get; set; }

    }
}
