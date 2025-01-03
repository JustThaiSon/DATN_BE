using DATN_Helpers.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.Models.Configurations
{
    public class SeatByShowTimeConfiguration : IEntityTypeConfiguration<SeatByShowTime>
    {
        public void Configure(EntityTypeBuilder<SeatByShowTime> builder)
        {
            builder.HasKey(x => x.SeatStatusByShowTimeId);

            builder.Property(x => x.ShowTimeId)
                .IsRequired();

            builder.Property(x => x.SeatId)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasDefaultValue(SeatStatusEnum.Available);


        }
    }
}
