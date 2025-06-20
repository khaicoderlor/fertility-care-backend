using FertilityCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations;

public class MedicalExaminationConfiguration : IEntityTypeConfiguration<MedicalExamination>
{
    public void Configure(EntityTypeBuilder<MedicalExamination> builder)
    {
        builder.ToTable("MedicalExamination");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1000, 1);
        builder.Property(x => x.AppointmentId).IsRequired();
        builder.Property(x => x.Symptoms).HasColumnType("NVARCHAR(MAX)").HasDefaultValue("");

        builder.Property(x => x.Diagnosis).HasColumnType("NVARCHAR(MAX)").HasDefaultValue("");

        builder.Property(x => x.Indications).HasColumnType("NVARCHAR(MAX)").HasDefaultValue("");

        builder.Property(x => x.Note).HasColumnType("NVARCHAR(MAX)").HasDefaultValue("");

        builder.HasOne(x => x.Appointment)
            .WithMany()
            .HasForeignKey(x => x.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_MedicalExamination_Appointment");
    }
}