using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class PendingPointRefundsConfiruation : IEntityTypeConfiguration<PendingPointRefund>
    {
        public void Configure(EntityTypeBuilder<PendingPointRefund> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(x => x.Email)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(x => x.OrderId)
                .IsRequired();

            builder.Property(x => x.Points)
                .IsRequired();

            builder.Property(x => x.IsClaimed)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.ClaimedUserId)
                .IsRequired(false);

            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
