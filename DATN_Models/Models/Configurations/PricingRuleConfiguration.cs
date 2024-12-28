using DATN_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Configurations
{
    public class PricingRuleConfiguration : IEntityTypeConfiguration<PricingRules>
    {
        public void Configure(EntityTypeBuilder<PricingRules> builder)
        {
            builder.HasKey(x => x.PricingRuleId);
            builder.Property(x => x.Multiplier)
                .IsRequired();
            builder.Property(x => x.RuleName)
               .IsRequired()
               .HasMaxLength(100);

        }
    }
}
