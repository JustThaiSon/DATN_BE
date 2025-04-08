using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class UserServiceHistoryConfiruation : IEntityTypeConfiguration<UserServiceHistory>
    {
        public void Configure(EntityTypeBuilder<UserServiceHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.ServiceId)
                .IsRequired();

            builder.Property(x => x.UsedDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.MembershipId)
                .IsRequired(false); 
        }
    }
}
