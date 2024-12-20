using DATN_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Configurations
{
    public class OrderDetailConfiruation : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ServiceId)
                .IsRequired();

            builder.Property(x => x.OrderDetailId)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.UnitPrice)
                .IsRequired();

            builder.Property(x => x.TotalPrice)
                .IsRequired();

            builder.Property(x => x.CreateDate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
