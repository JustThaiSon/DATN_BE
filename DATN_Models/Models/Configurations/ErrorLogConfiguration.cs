using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DATN_Models.Models.Configurations
{
    public class ErrorLogConfiguration : IEntityTypeConfiguration<ErrorLog>
    {
        public void Configure(EntityTypeBuilder<ErrorLog> builder)
        {
            builder.HasKey(t => t.ErrorLogID);
            builder.Property(t => t.ErrorMessage).HasMaxLength(4000);
            builder.Property(t => t.ErrorProcedure).HasMaxLength(126);
            builder.Property(t => t.HostName).HasMaxLength(200);
            builder.Property(t => t.UserName).HasColumnType("sysname").IsRequired();
            builder.Property(t => t.ErrorTime).HasDefaultValueSql("GETDATE()").IsRequired();
        }
    }
}
