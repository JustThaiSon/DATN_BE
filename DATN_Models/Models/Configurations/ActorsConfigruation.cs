using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class ActorsConfigruation : IEntityTypeConfiguration<Actors>
    {
        public void Configure(EntityTypeBuilder<Actors> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.DateOfBirth)
               .IsRequired();
            builder.Property(x => x.Biography)
                .HasMaxLength(1000);
            builder.Property(x => x.Photo)
                .HasMaxLength(500);
            builder.Property(x => x.Status)
                .HasDefaultValue(false);
        }
    }
}
