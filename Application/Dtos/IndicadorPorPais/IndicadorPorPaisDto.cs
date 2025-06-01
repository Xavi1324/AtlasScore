using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.IndicadorPorPais
{
    public class IndicadorPorPaisDto
    {
        public int Id { get; set; }
        public int PaisId { get; set; }
        public int MacroindicadorId { get; set; }
        public int Año { get; set; }
        public decimal Valor { get; set; }

        public string? NombrePais { get; set; }
        public string? NombreMacroindicador { get; set; }
    }
}
