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

            builder.Property(x => x.Multiplier)
                .IsRequired();

        }
    }
}
