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
    public class IndicadorPorPaisConfig : IEntityTypeConfiguration<IndicadorPorPais>
    {
        public void Configure(EntityTypeBuilder<IndicadorPorPais> builder)
        {
            builder.ToTable("IndicadoresPorPais");
            builder.HasKey(ip => ip.Id);

            builder.Property(ip => ip.Año).IsRequired();
            builder.Property(ip => ip.Valor).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasOne(ip => ip.Pais)
                   .WithMany(p => p.Indicadores)
                   .HasForeignKey(ip => ip.PaisId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ip => ip.Macroindicador)
                   .WithMany(m => m.Indicadores)
                   .HasForeignKey(ip => ip.MacroindicadorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(i => new { i.PaisId, i.MacroindicadorId, i.Año })
                   .IsUnique();
          

        }
    }

}
