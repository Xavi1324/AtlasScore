using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.Macroindicador
{
    public class MacroindicadorViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Range(0, 1)]
        public decimal Peso { get; set; }

        [Display(Name = "¿Es mejor mientras más alto?")]
        public bool EsMejorMasAlto { get; set; }

    }
}
