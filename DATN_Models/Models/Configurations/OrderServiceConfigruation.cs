using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class OrderServiceConfigruation : IEntityTypeConfiguration<OrderServices>
    {
        public void Configure(EntityTypeBuilder<OrderServices> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity)
                .IsRequired();
            builder.Property(x => x.OrderId)
               .IsRequired();

            builder.Property(x => x.TotalPrice)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();
        }
    }
}
