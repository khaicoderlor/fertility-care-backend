using FertilityCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patient");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.MedicalHistory)
            .HasColumnType("ntext");

        builder.Property(p => p.PartnerFullName)
            .HasMaxLength(500);

        builder.Property(p => p.PartnerEmail)
            .HasMaxLength(255);

        builder.Property(p => p.PartnerPhone)
            .HasMaxLength(20);

        builder.HasOne(p => p.UserProfile)
            .WithMany()
            .HasForeignKey(p => p.UserProfileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}