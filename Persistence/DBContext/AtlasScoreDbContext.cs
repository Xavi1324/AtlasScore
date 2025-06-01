using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System.Reflection;

namespace Persistence.DBContext
{
    public class AtlasScoreDbContext : DbContext
    {
        public AtlasScoreDbContext(DbContextOptions<AtlasScoreDbContext> options) : base(options) {}

        public DbSet<Pais> Paises { get; set; }
        public DbSet<Macroindicador> Macroindicadores { get; set; }
        public DbSet<IndicadorPorPais> IndicadoresPorPais { get; set; }
        public DbSet<TasaRetorno> TasasRetorno { get; set; }
        public DbSet<SimulacionMacroindicador> SimulacionesMacroindicadores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
    

