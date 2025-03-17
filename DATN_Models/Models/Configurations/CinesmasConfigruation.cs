using DATN_Helpers.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class CinesmasConfigruation : IEntityTypeConfiguration<Cinemas>
    {
        public void Configure(EntityTypeBuilder<Cinemas> builder)
        {
            builder.HasKey(x => x.CinemasId);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Address)
                .HasMaxLength(500);

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(15);

            builder.Property(x => x.TotalRooms)
                .IsRequired();
            builder.Property(x => x.Status)
                .HasDefaultValue(CinemaStatusEnum.Open);

            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
