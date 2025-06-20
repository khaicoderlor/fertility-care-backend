using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace FertilityCare.Infrastructure.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctor");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Degree)
            .HasMaxLength(255);

        builder.Property(d => d.Specialization)
            .HasColumnType("ntext");

        builder.Property(d => d.YearsOfExperience)
            .IsRequired(false);

        builder.Property(d => d.Biography)
            .HasMaxLength(2000);

        builder.Property(d => d.Rating)
            .HasColumnType("decimal(3,2)");

        builder.Property(d => d.PatientsServed)
            .IsRequired(false);

        builder.HasOne(d => d.UserProfile)
            .WithMany()
            .HasForeignKey(d => d.UserProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.DoctorSchedules)
            .WithOne(s => s.Doctor)
            .HasForeignKey(s => s.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
