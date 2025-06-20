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

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.CancellationReason)
            .HasMaxLength(1000);

        builder.Property(a => a.Note)
            .HasColumnType("ntext");

        builder.Property(a => a.ExtraFee)
            .HasColumnType("decimal(18,2)");

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(a => a.PaymentStatus)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(a => a.Patient)
            .WithMany()
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Doctor)
            .WithMany()
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.DoctorSchedule)
            .WithMany()
            .HasForeignKey(a => a.DoctorScheduleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.TreatmentService)
            .WithMany()
            .HasForeignKey(a => a.TreatmentServiceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.OrderStep)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.OrderStepId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}