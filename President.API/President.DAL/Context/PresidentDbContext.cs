using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using President.DAL.Entities;

namespace President.DAL.Context
{
    public class PresidentDbContext : IdentityDbContext<User>
    {
        public PresidentDbContext(DbContextOptions options)
            : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.PlayerStatistics)
                .WithOne(s => s.User)
                .HasForeignKey<PlayerStatistics>(s => s.UserId);
        }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<PlayerStatistics> PlayerStatistics { get; set; }
    }
}
