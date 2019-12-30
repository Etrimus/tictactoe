namespace Core.Models
{
    public class Player
    {
        public Player(Board board)
        {
            Board = board;
        }

        public CellType CellType { get; internal set; }

        public PlayerState State { get; internal set; }

        internal Board Board { get; }
    }

    public enum PlayerState
    {
        Undefined = 0,
        ChoiceType = 1,
        WaitingForTurn = 2,
        ReadyToTurn = 3,
        Waiting = 4,
        Winner = 5
    }
}