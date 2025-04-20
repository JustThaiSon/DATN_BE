using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DATN_Models.Models.Configurations
{
    public class PaymentHistoryConfiguration : IEntityTypeConfiguration<PaymentHistory>
    {
        public void Configure(EntityTypeBuilder<PaymentHistory> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.OrderId)
                   .IsRequired();

            builder.Property(t => t.PaymentMethodId)
                   .IsRequired();

            builder.Property(t => t.AmountPaid)
                   .IsRequired();

            builder.Property(t => t.PaymentDate)
                   .IsRequired();

            builder.Property(t => t.TransactionCode)
                   .HasMaxLength(200)
                   .IsRequired();
        }
    }
}
