﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class MovieFormatsConfigruation : IEntityTypeConfiguration<MovieFormats>
    {
        public void Configure(EntityTypeBuilder<MovieFormats> builder)
        {
            builder.HasKey(x => x.FormatId);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(200);



            builder.Property(x => x.Status)
                .HasDefaultValue(1);
        }
    }
}
