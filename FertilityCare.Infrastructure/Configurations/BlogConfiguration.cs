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
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("Blog");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(b => b.Content).IsRequired()
                .HasColumnType("NTEXT");

            builder.Property(b => b.Status);

            builder.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(b => b.UpdatedAt)
                .IsRequired(false);

            builder.Property(b => b.ImageUrl)
                .IsRequired(false);

            builder.HasOne(b => b.UserProfile)
                .WithMany()
                .HasForeignKey(b => b.UserProfileId);

        }
    }
}
