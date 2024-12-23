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
    public class CinesmasConfigruation : IEntityTypeConfiguration<Cinemas>
    {
        public void Configure(EntityTypeBuilder<Cinemas> builder)
        {
            builder.HasKey(x => x.CinemasId);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Address)
                .HasMaxLength(500);

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(15);

            builder.Property(x => x.TotalRooms)
                .IsRequired();
            builder.Property(x => x.Status)
                .HasDefaultValue(1);

            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
