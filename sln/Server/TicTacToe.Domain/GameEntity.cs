using TicTacToe.Core;

namespace TicTacToe.Domain;

public class GameEntity: DbEntityBase
{
    public Guid? CrossPlayerId { get; set; }

    public Guid? ZeroPlayerId { get; set; }

    public CellType NextTurn { get; set; }

    public CellType Winner { get; set; }

    public byte[] Cells { get; set; }
}