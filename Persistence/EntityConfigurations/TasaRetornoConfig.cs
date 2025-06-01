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
    public class TasaRetornoConfig : IEntityTypeConfiguration<TasaRetorno>
    {
        public void Configure(EntityTypeBuilder<TasaRetorno> builder)
        {
            builder.ToTable("TasasRetorno");
            builder.HasKey(tr => tr.Id);

            builder.Property(t => t.TasaMinima).IsRequired().HasColumnType("decimal(5,2)");
            builder.Property(t => t.TasaMaxima).IsRequired().HasColumnType("decimal(5,2)");
            
           
        }
    }
}
