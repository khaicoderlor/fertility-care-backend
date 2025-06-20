using FertilityCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations;

public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
{
    public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
    {
        builder.ToTable("DoctorSchedule");

        builder.HasKey(s => s.Id);

        //builder.Property(s => s.WorkDate)
        //    .HasConversion<DateOnlyConverter>()
        //    .IsRequired();

        builder.Property(s => s.MaxAppointments)
            .IsRequired(false);

        builder.Property(s => s.IsAcceptingPatients)
            .HasDefaultValue(true);

        builder.Property(s => s.Note)
            .HasMaxLength(1000);

        builder.Property(s => s.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(s => s.Doctor)
            .WithMany(d => d.DoctorSchedules)
            .HasForeignKey(s => s.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Slot)
            .WithMany()
            .HasForeignKey(s => s.SlotId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
