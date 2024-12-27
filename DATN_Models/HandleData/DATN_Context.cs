using DATN_Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
            optionsBuilder.UseSqlServer("Server=tcp:nhom1-datn.database.windows.net,1433;Initial Catalog=rapphim;Persist Security Info=False;User ID=datn;Password=Nghiagaming1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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
        public DbSet<Actors> Actors { get; set; }
        public DbSet<AppRoles> AppRoles { get; set; }
        public DbSet<AppUsers> AppUsers { get; set; }
        public DbSet<Cinemas> Cinemas { get; set; }
        public DbSet<Comments> Comments { get; set; }
        // public DbSet<Discounts> Discounts { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<GenresMovie> GenresMovie { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<MovieActors> MovieActors { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderServices> OrderServices { get; set; }
        // public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Ratings> Ratings { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<Seats> Seats { get; set; }
        public DbSet<SeatTypes> SeatTypes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ShowTime> ShowTimes { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<PricingRules> PricingRules { get; set; }
    }
}
