using DATN_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Configurations
{
    public class ActorMovieConfigruation : IEntityTypeConfiguration<MovieActors>
    {
        public void Configure(EntityTypeBuilder<MovieActors> builder)
        {
            builder.HasKey(x => new { x.MovieId, x.ActorId });
        }
    }
}
