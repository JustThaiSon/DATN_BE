using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class OptLogConfigruation : IEntityTypeConfiguration<OptLog>
    {
        public void Configure(EntityTypeBuilder<OptLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x=> x.Email).IsRequired();
            builder.Property(x => x.OtpCode).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
        }
    }
}
