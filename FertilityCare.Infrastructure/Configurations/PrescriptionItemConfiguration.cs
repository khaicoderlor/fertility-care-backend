using FertilityCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations;

public class PrescriptionItemConfiguration : IEntityTypeConfiguration<PrescriptionItem>
{
    public void Configure(EntityTypeBuilder<PrescriptionItem> builder)
    {
        builder.ToTable("PrescriptionItem");

        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.Id)
               .UseIdentityColumn(1000, 1);

        builder.Property(pi => pi.MedicationName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(pi => pi.StartDate)
               .HasColumnType("DATE")
               .HasDefaultValueSql("GETDATE()");

        builder.Property(pi => pi.EndDate)
               .HasColumnType("DATE");

        builder.Property(pi => pi.Quantity)
               .HasDefaultValue(1);

        builder.Property(pi => pi.SpecialInstructions)
               .HasColumnType("NTEXT");

        builder.HasOne(pi => pi.Prescription)
               .WithMany(p => p.PrescriptionItems)
               .HasForeignKey(pi => pi.PrescriptionId)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_PrescriptionItem_Prescription");
    }
}

