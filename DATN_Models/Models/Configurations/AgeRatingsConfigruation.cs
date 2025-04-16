﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class AgeRatingsConfigruation : IEntityTypeConfiguration<AgeRatings>
    {
        public void Configure(EntityTypeBuilder<AgeRatings> builder)
        {
            builder.HasKey(x => x.AgeRatingId);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.MinimumAge)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasDefaultValue(1);
        }
    }
}
