using JetBrains.Annotations;
using TicTacToe.Core.Services;

namespace TicTacToe.Core.Models
{
    [UsedImplicitly]
    public class Board
    {
        private readonly BoardManager _boardManager;

        public Board(ushort gridSize)
        {
            _boardManager = new BoardManager();

            Cells = new ReadOnlyTwoDimentionalCollection<Cell>(_boardManager.CreateCells(gridSize));
        }

        public ReadOnlyTwoDimentionalCollection<Cell> Cells { get; }

        public CellType NextTurn { get; internal set; } = CellType.Cross;

        public CellType Winner { get; internal set; } = CellType.None;

        public bool TryTurn(ushort cellNumber, out string result)
        {
            return _boardManager.TryTurn(this, cellNumber, out result);
        }
    }
}