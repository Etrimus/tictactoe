namespace Core.Models
{
    public class Board
    {
        internal Board(ushort gridSize)
        {
            Cells = new Cell[gridSize, gridSize];
        }

        public Cell[,] Cells { get; }

        public Player Player1 { get; internal set; }

        public Player Player2 { get; internal set; }
    }

    public enum BoardState
    {
        Undefined = 0,
        SetPlayers = 1,
        InGame = 2,
        End = 3
    }
}