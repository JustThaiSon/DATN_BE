using DATN_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Configurations
{
    public class OrderServiceConfigruation : IEntityTypeConfiguration<OrderServices>
    {
        public void Configure(EntityTypeBuilder<OrderServices> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.UnitPrice)
                .IsRequired();

            builder.Property(x => x.TotalPrice)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();
        }
    }
}
