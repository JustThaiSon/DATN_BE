using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class MembershipPurchaseConfiguration : IEntityTypeConfiguration<MembershipPurchase>
    {
        public void Configure(EntityTypeBuilder<MembershipPurchase> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.MembershipId)
                .IsRequired();

            builder.Property(x => x.Amount)
                .IsRequired();


            builder.Property(x => x.PaidAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
