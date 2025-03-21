using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class OrderConfigruation : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedDate)
                .IsRequired();
            builder.Property(x => x.UpdatedDate).IsRequired(false);
            builder.Property(x => x.TotalPrice)
                .IsRequired();
            builder.Property(x => x.Status)
                .IsRequired();
            builder.Property(x => x.OrderCode)
                .IsRequired();
            builder.Property(x => x.IsAnonymous)
                .IsRequired();
        }

    }
}
