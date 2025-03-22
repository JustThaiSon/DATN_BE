using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("Vouchers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.Code)
                .IsUnique();

            builder.Property(x => x.Description)
                .HasColumnType("nvarchar(500)");

            builder.Property(x => x.DiscountType)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.DiscountValue)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate)
                .IsRequired();

            builder.Property(x => x.MaxUsage)
                .IsRequired();

            builder.Property(x => x.UsedCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.UpdatedAt);

            // Relationships được định nghĩa trong VoucherUsageConfiguration
        }
    }
}