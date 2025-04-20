﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class UserVoucherConfiguration : IEntityTypeConfiguration<UserVoucher>
    {
        public void Configure(EntityTypeBuilder<UserVoucher> builder)
        {
            builder.ToTable("UserVouchers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.VoucherId)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.ClaimedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.ExpiryDate)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(x => x.UsedQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            // No relationships - NoSQL approach
        }
    }
}
