using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace DATN_Models.Models.Configurations
{
    public class ParamConfigConfiguration : IEntityTypeConfiguration<ParamConfig>
    {
        public void Configure(EntityTypeBuilder<ParamConfig> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.GroupConfigId)
                   .IsRequired();

            builder.Property(t => t.ParamType)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(t => t.ParamCode)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(t => t.ParamValue)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(t => t.Description)
                   .HasMaxLength(500);

            builder.Property(t => t.Status)
                   .IsRequired();
        }
    }
}
