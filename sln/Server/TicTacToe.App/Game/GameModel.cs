using TicTacToe.App.User;
using TicTacToe.Core.Models;

namespace TicTacToe.App.Game
{
    public class GameModel : ModelBase
    {
        [Obsolete("For Automapper only.", true)]
        public GameModel()
        { }

        public GameModel(Board board)
        {
            Board = board;
        }

        public UserModel CrossPlayer { get; set; }

        public UserModel ZeroPlayer { get; set; }

        public Board Board { get; set; }
    }
}