﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class ActorMovieConfigruation : IEntityTypeConfiguration<MovieActors>
    {
        public void Configure(EntityTypeBuilder<MovieActors> builder)
        {
            builder.HasKey(x => new { x.MovieId, x.ActorId });
        }
    }
}
