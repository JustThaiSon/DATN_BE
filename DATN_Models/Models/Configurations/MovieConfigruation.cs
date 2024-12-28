using DATN_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class MovieConfigruation : IEntityTypeConfiguration<Movies>
    {
        public void Configure(EntityTypeBuilder<Movies> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.MovieName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.Thumbnail)
                .HasMaxLength(500);

            builder.Property(x => x.Trailer)
                .HasMaxLength(500);

            builder.Property(x => x.Duration)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasDefaultValue(false);

            builder.Property(x => x.ReleaseDate)
                .IsRequired();
        }
    }
}
