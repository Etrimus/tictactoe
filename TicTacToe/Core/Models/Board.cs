using JetBrains.Annotations;
using TicTacToe.Core.Services;

namespace TicTacToe.Core.Models
{
    [UsedImplicitly]
    public class Board
    {
        public Board(ushort gridSize)
        {
            Cells = new ReadOnlyTwoDimensionalCollection<Cell>(BoardManager.CreateCells(gridSize));
        }

        public ReadOnlyTwoDimensionalCollection<Cell> Cells { get; }

        public CellType NextTurn { get; internal set; } = CellType.Cross;

        public CellType Winner { get; internal set; } = CellType.None;

        public bool TryTurn(ushort cellNumber, out TurnResult result)
        {
            return BoardManager.TryTurn(this, cellNumber, out result);
        }
    }
}