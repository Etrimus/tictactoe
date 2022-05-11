namespace TicTacToe.Core.Models;

public class Board
{
    internal Board(Cell[,] cells, CellType nextTurn = CellType.Cross, CellType winner = CellType.None)
    {
        Cells = new ReadOnlyTwoDimensionalCollection<Cell>(cells);
        NextTurn = nextTurn;
        Winner = winner;
    }

    public ReadOnlyTwoDimensionalCollection<Cell> Cells { get; }

    public CellType NextTurn { get; internal set; }

    public CellType Winner { get; internal set; }
}