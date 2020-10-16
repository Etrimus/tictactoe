using Core.Services;
using JetBrains.Annotations;

namespace Core.Models
{
    [UsedImplicitly]
    public class Board
    {
        private readonly BoardManager _boardManager;

        public Board(ushort gridSize)
        {
            _boardManager = new BoardManager();

            Cells = _boardManager.CreateCells(gridSize);
        }

        public Cell[,] Cells { get; }

        public CellType NextTurn { get; internal set; } = CellType.Cross;

        public CellType Winner { get; internal set; } = CellType.None;

        public bool TryTurn(ushort cellNumber, out string result)
        {
            return _boardManager.TryTurn(this, cellNumber, out result);
        }
    }
}