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
    public class MacroindicadorConfig : IEntityTypeConfiguration<Macroindicador>
    {
        public void Configure(EntityTypeBuilder<Macroindicador> builder)
        {
            builder.ToTable("Macroindicadores");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(m => m.Peso).IsRequired().HasColumnType("decimal(5,2)");
            builder.Property(m => m.EsMejorMasAlto).IsRequired();

            builder.HasMany(m => m.Indicadores)
                   .WithOne(i => i.Macroindicador)
                   .HasForeignKey(i => i.MacroindicadorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.Simulaciones)
                   .WithOne(s => s.Macroindicador)
                   .HasForeignKey(s => s.MacroindicadorId)
                   .OnDelete(DeleteBehavior.Cascade);

            
        }
    }

}
