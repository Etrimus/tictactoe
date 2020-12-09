using System;
using TicTacToe.Core;

namespace TicTacToe.Domain
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GameEntity : DbEntityBase
    {
        public Guid? CrossId { get; set; }

        public Guid? ZeroId { get; set; }

        public CellType NextTurn { get; set; }

        public CellType Winner { get; set; }

        public byte[] Cells { get; set; }
    }
}