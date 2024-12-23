using DATN_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.Models.Configurations
{
    public class MembershipConfiruation : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(x => x.Description)
                .HasMaxLength(1000);
            builder.Property(x => x.DiscountPercentage)
                .IsRequired();
            builder.Property(x => x.MonthlyFee)
                .IsRequired();
            builder.Property(x => x.DurationMonths)
                .IsRequired();
            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.Status)
                .HasDefaultValue(false);
        }
    }
}
