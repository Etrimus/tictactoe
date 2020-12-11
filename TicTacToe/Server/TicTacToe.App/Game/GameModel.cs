using System;
using JetBrains.Annotations;
using TicTacToe.Core.Models;

namespace TicTacToe.App.Game
{
    public class GameModel : ModelBase
    {
        [Obsolete("For Automapper only.", true)]
        [UsedImplicitly]
        public GameModel()
        { }

        public GameModel(Board board)
        {
            Board = board;
        }

        public Guid? CrossId { get; set; }

        public Guid? ZeroId { get; set; }

        public Board Board { get; set; }
    }
}