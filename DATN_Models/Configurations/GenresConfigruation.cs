using DATN_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Configurations
{
    public class GenresConfigruation : IEntityTypeConfiguration<Genres>
    {
        public void Configure(EntityTypeBuilder<Genres> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.GenreName).HasMaxLength(1000).IsRequired();
        }
    }
}
