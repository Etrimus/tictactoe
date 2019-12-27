namespace TicTacToe
{
    public class Cell
    {
        public CellState State { get; set; }
    }

    public enum CellState
    {
        Empty = 0,
        Zero = 1,
        Cross = 2
    }
}