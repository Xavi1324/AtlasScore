using Persistence.Common;

namespace Persistence.Entities
{
    public class Pais : BaseEntity<int>
    {
        public string Nombre { get; set; }

        public string CodigoIso { get; set; }

        public ICollection<IndicadorPorPais> Indicadores { get; set; } = new List<IndicadorPorPais>();


    }
}
