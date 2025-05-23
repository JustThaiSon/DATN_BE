﻿using DATN_Helpers.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class RoomConfigruation : IEntityTypeConfiguration<Rooms>
    {
        public void Configure(EntityTypeBuilder<Rooms> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(x => x.TotalColNumber)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(x => x.TotalRowNumber)
               .HasMaxLength(100)
               .IsRequired();
            builder.Property(x => x.Status)
                .HasDefaultValue(RoomStatusEnum.Available);
            builder.Property(x => x.SeatPrice)
                .IsRequired();

        }
    }
}
