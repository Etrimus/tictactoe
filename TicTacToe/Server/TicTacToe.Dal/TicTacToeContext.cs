using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TicTacToe.Domain;

namespace TicTacToe.Dal
{
    public class TicTacToeContext : DbContext
    {
        public DbSet<GameEntity> Game { get; set; }

        public DbSet<UserEntity> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            opt
                .UseSqlite("Data Source=TicTacToe.sqlite")
                .LogTo(x => Debug.WriteLine(x), new[] { RelationalEventId.CommandExecuted })
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
    }
}