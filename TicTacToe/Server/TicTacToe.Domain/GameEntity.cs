using System;

namespace TicTacToe.Domain
{
    public class GameEntity : DbEntityBase
    {
        public Guid? CrossId { get; set; }

        public Guid? ZeroId { get; set; }
    }
}