using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class UserPointConfiguration : IEntityTypeConfiguration<UserPoint>
    {
        public void Configure(EntityTypeBuilder<UserPoint> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();
            builder.Property(x => x.TotalPoint)
                .IsRequired();

            builder.Property(x => x.UpdatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Status)
                .HasDefaultValue(1); 
        }
    }
}
