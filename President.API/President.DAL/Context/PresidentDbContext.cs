﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using President.DAL.Entities;

namespace President.DAL.Context
{
    public class PresidentDbContext : IdentityDbContext<User>
    {
        public PresidentDbContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameStatistics> GameStatistics { get; set; }
        public DbSet<PlayerStatistics> PlayerStatistics { get; set; }
        public DbSet<PlayerGameStatistics> PlayerGameStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlayerGameStatistics>()
                .HasKey(pgs => new { pgs.PlayerStatisticsId, pgs.GameStatisticsId });

            modelBuilder.Entity<PlayerGameStatistics>()
                .HasOne(pgs => pgs.PlayerStatistics)
                .WithMany(u => u.PlayerGameStatistics)
                .HasForeignKey(pgs => pgs.PlayerStatisticsId);

            modelBuilder.Entity<PlayerGameStatistics>()
                .HasOne(pgs => pgs.GameStatistics)
                .WithMany(gs => gs.PlayerGameStatistics)
                .HasForeignKey(pgs => pgs.GameStatisticsId);
        }
    }
}
