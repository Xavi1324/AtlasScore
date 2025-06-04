using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.Pais
{
    public class PaisViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo Nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Código ISO es obligatorio.")]
        [StringLength(10)]
        public string CodigoIso { get; set; }
       
    }
}
