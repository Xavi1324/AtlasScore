using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntityConfigurations
{
    public class SimulacionMacroindicadorConfig : IEntityTypeConfiguration<SimulacionMacroindicador>
    {
        public void Configure(EntityTypeBuilder<SimulacionMacroindicador> builder)
        {
            builder.ToTable("SimulacionesMacroindicadores");
            builder.HasKey(sm => sm.Id);

            builder.Property(s => s.PesoSimulacion).IsRequired().HasColumnType("decimal(5,2)");

            builder.HasOne(s => s.Macroindicador)
                   .WithMany(m => m.Simulaciones)
                   .HasForeignKey(s => s.MacroindicadorId)
                   .OnDelete(DeleteBehavior.Cascade);
           
        }
    }
}
