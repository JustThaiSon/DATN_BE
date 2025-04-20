﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class MovieFormats_MoviesConfigruation : IEntityTypeConfiguration<MovieFormats_Movies>
    {
        public void Configure(EntityTypeBuilder<MovieFormats_Movies> builder)
        {
            builder.HasKey(x => new { x.MovieId, x.FormatId });

            builder.Property(x => x.MovieId)
                .IsRequired();

            builder.Property(x => x.FormatId)
                .IsRequired();
        }
    }
}
