﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class VoucherUIConfiguration : IEntityTypeConfiguration<VoucherUI>
    {
        public void Configure(EntityTypeBuilder<VoucherUI> builder)
        {
            builder.ToTable("VoucherUIs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.VoucherId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Content)
                .HasColumnType("nvarchar(1000)");

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.EndTime)
                .IsRequired();

            builder.Property(x => x.status)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.UpdatedAt);

            // No relationships - NoSQL approach
        }
    }
}
