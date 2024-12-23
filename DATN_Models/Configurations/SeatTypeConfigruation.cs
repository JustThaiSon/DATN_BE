using DATN_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Configurations
{
    internal class SeatTypeConfigruation : IEntityTypeConfiguration<SeatTypes>
    {
        public void Configure(EntityTypeBuilder<SeatTypes> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Price)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();
        }
    }
}
