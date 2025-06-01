using Persistence.Common;

namespace Persistence.Entities
{
    public class Macroindicador : BaseEntity<int>
    {
        public string Nombre { get; set; }

        public decimal Peso { get; set; }

        public bool EsMejorMasAlto { get; set; }

        public ICollection<IndicadorPorPais> Indicadores { get; set; } = new List<IndicadorPorPais>();

        public ICollection<SimulacionMacroindicador> Simulaciones { get; set; } = new List<SimulacionMacroindicador>();

    }
}
