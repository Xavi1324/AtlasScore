using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.TasaRetorno
{
    public class TasaRetornoViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = " La tasa minima es requerida.")]
        [Range(0, double.MaxValue, ErrorMessage = "La tasa minima debe ser un número positivo.")]
        [Display(Name = "Tasa Mínima estimada de retorno")]
        public decimal TasaMinima { get; set; }
        [Required(ErrorMessage = " La tasa maxima es requerida.")]
        [Range(0, double.MaxValue, ErrorMessage = "La tasa maxima debe ser un número positivo.")]
        [Display(Name = "Tasa Máxima estimada de retorno")]
        public decimal TasaMaxima { get; set; }
    }
}
