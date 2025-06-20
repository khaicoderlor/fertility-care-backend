using FertilityCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations
{
    public class OrderStepPaymentConfiguration : IEntityTypeConfiguration<OrderStepPayment>
    {
        public void Configure(EntityTypeBuilder<OrderStepPayment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PaymentCode)
                .HasMaxLength(255);

            builder.Property(x => x.TransactionCode)
                .HasMaxLength(255);

            builder.Property(x => x.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.GatewayResponseCode)
                .HasMaxLength(50);

            builder.Property(x => x.GatewayMessage)
                .HasMaxLength(255);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.PaymentDate)
                .IsRequired();

            builder.HasOne(x => x.Patient)
                .WithMany()
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.OrderStep)
                .WithMany()
                .HasForeignKey(x => x.OrderStepId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}