using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class UserMembershipConfiguration : IEntityTypeConfiguration<UserMembership>
    {
        public void Configure(EntityTypeBuilder<UserMembership> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.MembershipId)
                .IsRequired();

            builder.Property(x => x.PurchasedAt)
                .IsRequired();

            builder.Property(x => x.ExpiryDate)
                .IsRequired();
            builder.Property(x => x.MemberCode)
             .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasDefaultValue(0);
        }
    }
}
