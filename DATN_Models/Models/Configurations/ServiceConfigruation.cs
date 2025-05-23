﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class ServiceConfigruation : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Price)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();
        }
    }
}

