using TicTacToe.Core;

namespace TicTacToe.Domain
{
    public class GameEntity : DbEntityBase
    {
        public UserEntity CrossPlayer { get; set; }

        public UserEntity ZeroPlayer { get; set; }

        public CellType NextTurn { get; set; }

        public CellType Winner { get; set; }

        public byte[] Cells { get; set; }
    }
}