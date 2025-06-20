using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FertilityCare.Infrastructure.Configurations
{

    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedback");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .HasDefaultValueSql("NEWID()");

            builder.HasOne(f => f.Patient)
                .WithMany()
                .HasForeignKey(f => f.PatientId);

            builder.HasOne(f => f.Doctor)
                .WithMany()
                .HasForeignKey(f => f.DoctorId);

            builder.HasOne(f => f.TreatmentService)
                .WithMany()
                .HasForeignKey(f => f.TreatmentServiceId);

            builder.Property(f => f.Status);

            builder.Property(f => f.Rating)
                .HasColumnType("DECIMAL(3,2)")
                .IsRequired();

            builder.Property(f => f.Comment)
                .HasColumnType("NVARCHAR(MAX)")
                .IsRequired(false);

            builder.Property(f => f.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(f => f.UpdatedAt);

        }
    }
}
