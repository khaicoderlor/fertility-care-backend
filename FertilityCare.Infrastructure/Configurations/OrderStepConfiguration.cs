using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations;

public class OrderStepConfiguration : IEntityTypeConfiguration<OrderStep>
{
    public void Configure(EntityTypeBuilder<OrderStep> builder)
    {
        builder.ToTable("OrderStep");

        builder.HasKey(os => os.Id);

        builder.Property(os => os.Status)
            .IsRequired();

        builder.Property(os => os.StartDate)
            //.HasConversion<DateOnlyConverter>()
            .IsRequired();

        builder.Property(os => os.EndDate)
            //.HasConversion<DateOnlyConverter>()
            .IsRequired(false);

        builder.HasOne(os => os.Order)
            .WithMany(o => o.OrderSteps)
            .HasForeignKey(os => os.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(os => os.TreatmentStep)
            .WithMany()
            .HasForeignKey(os => os.TreatmentStepId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}