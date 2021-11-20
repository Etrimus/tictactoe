using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TicTacToe.Domain;

namespace TicTacToe.Dal
{
    public class TicTacToeContext : DbContext
    {
        public DbSet<GameEntity> Game { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            opt
                .UseSqlite("Data Source=TicTacToe.sqlite")
                .LogTo(x => Debug.WriteLine(x), new[] { RelationalEventId.CommandExecuted })
                .EnableSensitiveDataLogging()
                //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserEntity>()
                .HasIndex(x => x.Name).IsUnique(true);

            builder.Entity<UserEntity>()
                .Property(x => x.Name)
                .IsRequired(true);

            builder.Entity<UserEntity>()
                .Property(x => x.Password)
                .IsRequired(true);
        }
    }
}