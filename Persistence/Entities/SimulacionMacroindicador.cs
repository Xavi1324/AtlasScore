using Persistence.Common;

namespace Persistence.Entities
{
    public class SimulacionMacroindicador : BaseEntity<int>
    {
        public int MacroindicadorId { get; set; }
        public decimal PesoSimulacion { get; set; }
        public Macroindicador? Macroindicador { get; set; }
    }
}
