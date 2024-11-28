using DATN_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.Configurations
{
    public class ShowTimeConfigruation : IEntityTypeConfiguration<ShowTime>
    {
        public void Configure(EntityTypeBuilder<ShowTime> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.MovieId)
                .IsRequired(); 

            builder.Property(x => x.RoomId)
                .IsRequired();

            builder.Property(x => x.StartTime)
                .IsRequired(); 

            builder.Property(x => x.EndTime)
                .IsRequired(); 

            builder.Property(x => x.Status)
                .IsRequired();
        }
    }
}
