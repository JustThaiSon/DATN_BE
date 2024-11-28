using DATN_Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.HandleData
{
    public class DATN_Context : IdentityDbContext<AppUsers, AppRoles, Guid>
    {
        public DATN_Context()
        {
        }

        public DATN_Context(DbContextOptions options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=LAPTOP-APKREC8K\\SQLEXPRESS;Initial Catalog=DATN;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);
        }
    }
}
