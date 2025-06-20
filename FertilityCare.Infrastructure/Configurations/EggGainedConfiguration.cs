using FertilityCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations
{
    public class EggGainedConfiguration : IEntityTypeConfiguration<EggGained>
    {
        public void Configure(EntityTypeBuilder<EggGained> builder)
        {
            builder.ToTable("EggGained");

            builder.HasKey(e => e.Id);

            builder.HasOne(x => x.Order)
                .WithMany(e => e.EggGaineds)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
