namespace Core.Models
{
    public class Board
    {
        internal Board(ushort gridSize)
        {
            Cells = new Cell[gridSize, gridSize];
        }

        public Cell[,] Cells { get; }

        public CellType NextTurn { get; internal set; } = CellType.Cross;

        public CellType Winner { get; internal set; }
    }
}