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
                .UseInMemoryDatabase("TicTacToeDb")
                //.UseSqlite("Data Source=TicTacToe.sqlite")
                .LogTo(x => Debug.WriteLine(x), new[] { RelationalEventId.CommandExecuted })
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        { }
    }
}