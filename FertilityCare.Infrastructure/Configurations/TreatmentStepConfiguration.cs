using FertilityCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations;

public class TreatmentStepConfiguration : IEntityTypeConfiguration<TreatmentStep>
{
    public void Configure(EntityTypeBuilder<TreatmentStep> builder)
    {
        builder.ToTable("TreatmentStep");

        builder.HasKey(ts => ts.Id);

        builder.Property(ts => ts.StepName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(ts => ts.Description)
            .HasMaxLength(1000);

        builder.Property(ts => ts.StepOrder)
            .IsRequired();

        builder.Property(ts => ts.EstimatedDurationDays)
            .IsRequired(false);

        builder.Property(ts => ts.Amount)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(ts => ts.TreatmentService)
               .WithMany(t => t.TreatmentSteps) 
               .HasForeignKey(ts => ts.TreatmentServiceId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}
