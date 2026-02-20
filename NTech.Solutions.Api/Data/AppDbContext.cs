using Microsoft.EntityFrameworkCore;
using NTech.Solutions.Common.Models.Database;

namespace NTech.Solutions.Api.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public AppDbContext(IConfiguration config, DbContextOptions<AppDbContext> options) : base(options)
        {
            this._config = config;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Identity> Identities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseNpgsql(_config.GetConnectionString("PostgresDb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the User entity
            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Id, u.Email })
                .IsUnique();

            // Configure the relationship between User and RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the relationship between User and Identity
            modelBuilder.Entity<Identity>()
                .HasOne(i => i.User)
                .WithMany(u => u.Identities)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the Provider property to be stored as a string in the database
            modelBuilder.Entity<Identity>()
                .Property(i => i.Provider)
                .HasConversion<string>();
        }
    }
}
