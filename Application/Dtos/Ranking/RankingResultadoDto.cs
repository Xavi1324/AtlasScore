using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Ranking
{
    public class RankingResultadoDto
    {
        public int PaisId { get; set; }
        public string Pais { get; set; }
        public string CodigoIso { get; set; }
        public decimal Scoring { get; set; }
        public decimal TasaEstimacion { get; set; }
    }
}
