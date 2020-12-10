using System;
using TicTacToe.Core;
using TicTacToe.Core.Models;

namespace TicTacToe.App.Game
{
    public class GameModel : ModelBase
    {
        public Guid? CrossId { get; set; }

        public Guid? ZeroId { get; set; }

        public CellType NextTurn { get; set; }

        public CellType Winner { get; set; }

        public Cell[,] Cells { get; set; }
    }
}