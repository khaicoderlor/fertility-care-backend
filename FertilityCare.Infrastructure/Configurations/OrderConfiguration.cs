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

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.StartDate)
            //.HasConversion<DateOnlyConverter>()
            .IsRequired();

        builder.Property(o => o.EndDate)
            .IsRequired(false);

        builder.Property(o => o.Status)
            .IsRequired();

        builder.Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false);

        builder.Property(o => o.TotalEgg)
            .IsRequired(false);

        builder.HasOne(o => o.Patient)
            .WithMany()
            .HasForeignKey(o => o.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Doctor)
            .WithMany()
            .HasForeignKey(o => o.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.TreatmentService)
            .WithMany()
            .HasForeignKey(o => o.TreatmentServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.OrderSteps)
            .WithOne(s => s.Order)
            .HasForeignKey(s => s.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}