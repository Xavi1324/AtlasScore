using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.IndicadorPorPais
{
    public class IndicadorPorPaisViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El pais es obligatorio.")]
        [Display(Name = "País")]
        public int PaisId { get; set; }

        [Required(ErrorMessage = "El macroindicador es obligatorio.")]
        [Display(Name = "Macroindicador")]
        public int MacroindicadorId { get; set; }

        [Required(ErrorMessage = "El año es obligatorio.")]
        [Range(1900, 2100, ErrorMessage = "El año debe ser valido")]
        public int Año { get; set; }

        [Required(ErrorMessage = "El valor es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor debe ser un número positivo.")]
        public decimal Valor { get; set; }

        public string? NombrePais { get; set; }
        public string? NombreMacroindicador { get; set; }

    }
}
