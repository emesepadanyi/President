using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using President.DAL.Entities;

namespace President.DAL.Context
{
    public class PresidentDbContext : IdentityDbContext<User>
    {
        public PresidentDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameStatistics> GameStatistics { get; set; }
        public DbSet<PlayerStatistics> PlayerStatistics { get; set; }
    }
}
