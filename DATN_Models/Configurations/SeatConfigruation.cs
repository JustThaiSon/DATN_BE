using DATN_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Configurations
{
    public class SeatConfigruation : IEntityTypeConfiguration<Seats>
    {
        public void Configure(EntityTypeBuilder<Seats> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.NameSeat)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
