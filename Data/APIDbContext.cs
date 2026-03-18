using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class APIDbContext : DbContext
    {

        public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
        public DbSet<SystemRoles> SystemRoles => Set<SystemRoles>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<Hash> Hashes => Set<Hash>();

        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

#if DEBUG
            optionsBuilder.LogTo(Console.WriteLine);
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SystemRoles>().HasData(SystemRolles());
            modelBuilder.Entity<Hash>().HasKey(r => r.Id);
            modelBuilder.Entity<Hash>().HasIndex(h => h.UserId).IsUnique();
            modelBuilder.Entity<Hash>().HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Hash>(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefreshToken>().HasKey(r => r.Id);
            modelBuilder.Entity<RefreshToken>().HasIndex(h => h.UserId).IsUnique();
            modelBuilder.Entity<RefreshToken>().HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<RefreshToken>(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }

        private List<SystemRoles> SystemRolles()
        {
            List<SystemRoles> roles =
            [
                new SystemRoles { Name = "administrator", Kode = 1800, Id = Guid.Parse( "08beacc0-38dd-42a9-82c1-c3706a0cf19e") },
                new SystemRoles { Name = "organisatör", Kode = 1880, Id =  Guid.Parse( "6ac343b0-00ef-4a1c-8f64-68daaca77b5b" ) },
                new SystemRoles { Name = "user", Kode = 1900, Id =  Guid.Parse( "941fe147-6f5a-44bd-8c9a-6cf255b57c02" )},
            ];

            return roles;
        }

    }
}
