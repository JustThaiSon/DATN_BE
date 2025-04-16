using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class VoucherUsageConfiguration : IEntityTypeConfiguration<VoucherUsage>
    {
        public void Configure(EntityTypeBuilder<VoucherUsage> builder)
        {
            builder.ToTable("VoucherUsages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.VoucherId)
                .IsRequired();

            builder.Property(x => x.UsedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Status)
                .IsRequired();

            // No relationships - NoSQL approach
        }
    }
}