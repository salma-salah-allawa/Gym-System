using GymSystemApp.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymSystemApp.Contexts
{
    public class GymAppDbContext:IdentityDbContext<ApplicationUser>
    {
        public GymAppDbContext(DbContextOptions<GymAppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<GymUser> GymUsers { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Category> Categories { get; set; }
        
    }
}
