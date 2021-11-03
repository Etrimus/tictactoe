using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TicTacToe.Dal
{
    public class TicTacToeContext : DbContext
    {
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