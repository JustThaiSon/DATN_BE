using DATN_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
