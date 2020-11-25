using System;

namespace TicTacToe.App.Games
{
    public class Game : ModelBase
    {
        public Guid? CrossId { get; set; }

        public Guid? ZeroId { get; set; }
    }
}