using FertilityCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfile");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");

        builder.Property(x => x.FirstName).HasColumnType("NVARCHAR(255)");

        builder.Property(x => x.LastName).HasColumnType("NVARCHAR(255)");

        builder.Property(x => x.MiddleName).HasColumnType("NVARCHAR(255)");

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
    }
}
