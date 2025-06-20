using FertilityCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations
{
    public class EmbryoGainedConfiguration : IEntityTypeConfiguration<EmbryoGained>
    {
        public void Configure(EntityTypeBuilder<EmbryoGained> builder)
        {
            builder.ToTable("EmbryoGained");

            builder.HasKey(e => e.Id);

            builder.HasOne(x => x.Order)
                .WithMany(h => h.EmbryoGaineds)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
                
        }
    }
}
