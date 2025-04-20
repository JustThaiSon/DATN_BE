using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class MembershipBenefitConfiguration : IEntityTypeConfiguration<MembershipBenefit>
    {
        public void Configure(EntityTypeBuilder<MembershipBenefit> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.MembershipId)
                .IsRequired();

            builder.Property(x => x.BenefitType)
                .IsRequired()
                .HasMaxLength(250);
            builder.Property(x => x.LogoUrl)
                .IsRequired()
                .HasMaxLength(250);


            builder.Property(x => x.ConfigJson)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1000);
        }
    }
}
