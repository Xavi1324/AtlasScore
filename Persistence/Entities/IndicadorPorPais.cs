using Persistence.Common;

namespace Persistence.Entities
{
    public class IndicadorPorPais : BaseEntity<int>
    {
        public int PaisId { get; set; }
        public int MacroindicadorId { get; set; }
        public int Año { get; set; }
        public decimal Valor { get; set; }
        
        public Pais? Pais { get; set; }
        public Macroindicador? Macroindicador { get; set; }
    }
}
