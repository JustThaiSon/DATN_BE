using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class UserPointHistoryConfiruation : IEntityTypeConfiguration<UserPointHistory>
    {
        public void Configure(EntityTypeBuilder<UserPointHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.OrderId)
                .IsRequired(false); 

            builder.Property(x => x.PointChange)
                .IsRequired();

            builder.Property(x => x.Reason)
                .HasMaxLength(255) 
                .IsRequired(false); 

            builder.Property(x => x.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
