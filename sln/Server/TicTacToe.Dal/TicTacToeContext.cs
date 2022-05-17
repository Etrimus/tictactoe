using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain;

namespace TicTacToe.Dal;

public class TicTacToeContext: DbContext
{
    //private readonly BoardManager _boardManager = new();

    public TicTacToeContext()
    {
        Database.EnsureCreated();
    }

    [UsedImplicitly]
    public DbSet<GameEntity> Game { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder opt)
    {
        opt
            .UseInMemoryDatabase("TicTacToeDb")
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //var cells = _boardManager.CreateBoard(3).Cells.Select(x => (byte)x.State).ToArray();

        //builder.Entity<GameEntity>().HasData(
        //    new()
        //    {
        //        Id = Guid.Parse("3D225EFE-AD5B-4675-BFDB-CB6E30283EF1"),
        //        Cells = cells,
        //        NextTurn = Core.CellType.Cross
        //    },
        //    new()
        //    {
        //        Id = Guid.Parse("EFAEBFC6-723A-476B-88EB-FE7BECCF3DE7"),
        //        Cells = cells,
        //        NextTurn = Core.CellType.Cross,
        //        CrossPlayerId = Guid.Parse("0CE798CB-12E4-4876-8B0D-46D9B0F08D3F")
        //    },
        //    new()
        //    {
        //        Id = Guid.Parse("DCBDFE86-B420-4A1F-B5FD-43953188AE45"),
        //        Cells = cells,
        //        NextTurn = Core.CellType.Cross,
        //        CrossPlayerId = Guid.Parse("0CE798CB-12E4-4876-8B0D-46D9B0F08D3F"),
        //        ZeroPlayerId = Guid.Parse("418AA287-3D07-4D51-B370-51B45573C9CD")
        //    }
        //);
    }
}