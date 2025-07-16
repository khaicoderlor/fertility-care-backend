using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations;

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.ToTable("Prescription");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.PrescriptionDate)
               .HasColumnType("DATETIME")
               .HasDefaultValueSql("GETDATE()");

        builder.HasOne(x => x.Order)
               .WithMany()
               .HasForeignKey(x => x.OrderId)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_Prescription_TreatmentPlan");

        builder.HasMany(x => x.PrescriptionItems)
               .WithOne(x => x.Prescription)
               .HasForeignKey(pi => pi.PrescriptionId)
               .OnDelete(DeleteBehavior.Cascade)
               .HasConstraintName("FK_PrescriptionItem_Prescription");
    }
}
