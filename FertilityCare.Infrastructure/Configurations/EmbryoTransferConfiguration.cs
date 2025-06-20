using FertilityCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations;

public class EmbryoTransferConfiguration : IEntityTypeConfiguration<EmbryoTransfer>
{
    public void Configure(EntityTypeBuilder<EmbryoTransfer> builder)
    {
        builder.ToTable("EmbryoTransfer");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.TransferType)
            .IsRequired();

        builder.Property(e => e.TransferDate)
            .IsRequired();

        builder.HasOne(e => e.Appointment)
            .WithMany()
            .HasForeignKey(e => e.AppointmentId)
            .OnDelete(DeleteBehavior.SetNull);


        builder.HasOne(x => x.Order)
            .WithMany(o => o.EmbryoTransfers)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

    }

}
