using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Simulador
{
    public class PesoMacroindicadorSimuladoDto
    {
        public int Id { get; set; }
        public int MacroindicadorId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Peso { get; set; }
        public bool EsMejorMasAlto { get; set; }
    }
}
