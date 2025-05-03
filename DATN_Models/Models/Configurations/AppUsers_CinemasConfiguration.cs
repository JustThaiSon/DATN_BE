﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN_Models.Models.Configurations
{
    public class AppUsers_CinemasConfiguration : IEntityTypeConfiguration<AppUsers_Cinemas>
    {
        public void Configure(EntityTypeBuilder<AppUsers_Cinemas> builder)
        {
            builder.HasKey(x => new { x.UserId, x.CinemasId });

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.CinemasId)
                .IsRequired();
        }
    }
}
