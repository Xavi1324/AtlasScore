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
    public class PaisConfig : IEntityTypeConfiguration<Pais>
    {
        public void Configure(EntityTypeBuilder<Pais> builder)
        {
            builder.ToTable("Paises");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(p => p.CodigoIso).IsRequired().HasMaxLength(10);

            builder.HasMany(p => p.Indicadores)
                   .WithOne(i => i.Pais)
                   .HasForeignKey(i => i.PaisId)
                   .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
