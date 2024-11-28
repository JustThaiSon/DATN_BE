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
    public class ActorsConfigruation : IEntityTypeConfiguration<Actors>
    {
        public void Configure(EntityTypeBuilder<Actors> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Name).HasMaxLength(100);
            builder.Property(x => x.DateOfBirth)
               .IsRequired();
            builder.Property(x => x.Biography)
                .HasMaxLength(1000); 
            builder.Property(x => x.Photo)
                .HasMaxLength(500); 
            builder.Property(x => x.Status)
                .HasDefaultValue(false);
        }
    }
}
