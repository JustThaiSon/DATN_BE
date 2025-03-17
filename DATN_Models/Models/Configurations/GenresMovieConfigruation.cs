using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class GenresMovieConfigruation : IEntityTypeConfiguration<GenresMovie>
    {
        public void Configure(EntityTypeBuilder<GenresMovie> builder)
        {
            builder.HasKey(x => new { x.MovieId, x.GenresId });
        }
    }
}
