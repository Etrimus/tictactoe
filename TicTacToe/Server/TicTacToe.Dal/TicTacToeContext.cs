using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain;

namespace TicTacToe.Dal
{
    public class TicTacToeContext : DbContext
    {
        public DbSet<GameEntity> Game { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            opt
                //.UseInMemoryDatabase("TicTacToe")
                .UseSqlite("Data Source=TicTacToe.sqlite")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
    }
}