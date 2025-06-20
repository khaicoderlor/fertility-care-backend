using FertilityCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations;

public class TreatmentServiceConfiguration : IEntityTypeConfiguration<TreatmentService>
{
    public void Configure(EntityTypeBuilder<TreatmentService> builder)
    {
        builder.ToTable("TreatmentService");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
               .HasDefaultValueSql("NEWID()");

        builder.Property(t => t.Name)
               .IsRequired()
               .HasColumnType("NVARCHAR(MAX)");

        builder.Property(t => t.Description)
               .HasColumnType("NTEXT");

        builder.Property(t => t.EstimatePrice)
               .HasColumnType("DECIMAL(18,2)");

        builder.Property(t => t.Duration)
               .HasColumnType("INT");

        builder.Property(t => t.SuccessRate)
               .HasColumnType("DECIMAL(5,2)");

        builder.Property(t => t.RecommendedFor)
               .HasColumnType("NVARCHAR(MAX)");

        builder.Property(t => t.Contraindications)
               .HasColumnType("NVARCHAR(MAX)");

        builder.Property(t => t.UpdatedAt)
               .HasColumnType("DATETIME");

        builder.HasMany(t => t.TreatmentSteps)
                .WithOne(x => x.TreatmentService)
                .HasForeignKey(ts => ts.TreatmentServiceId)
                .OnDelete(DeleteBehavior.Cascade);

    }
}
