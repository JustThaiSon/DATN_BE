using DATN_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class ChangeLogConfigruation : IEntityTypeConfiguration<ChangeLog>
    {
        public void Configure(EntityTypeBuilder<ChangeLog> builder)
        {
            builder.HasKey(e => e.Id); 

            builder.Property(e => e.Action)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(e => e.TableName)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(e => e.BeforeChange)
                  .HasColumnType("nvarchar(max)");

            builder.Property(e => e.AfterChange)
                  .HasColumnType("nvarchar(max)");

            builder.Property(e => e.ChangeDateTime)
                  .HasDefaultValueSql("GETDATE()");
        }
    }
}
