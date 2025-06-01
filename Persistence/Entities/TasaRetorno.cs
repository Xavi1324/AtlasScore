using Persistence.Common;

namespace Persistence.Entities
{
    public class TasaRetorno: BaseEntity<int>
    {
        public decimal TasaMinima { get; set; }
        public decimal TasaMaxima { get; set; }
    }
}
