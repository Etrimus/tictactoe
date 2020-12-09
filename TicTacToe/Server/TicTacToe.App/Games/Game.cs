using System;
using System.Collections.Generic;
using TicTacToe.Core;
using TicTacToe.Core.Models;

namespace TicTacToe.App.Games
{
    public class Game : ModelBase
    {
        public Guid? CrossId { get; set; }

        public Guid? ZeroId { get; set; }

        public CellType NextTurn { get; set; }

        public CellType Winner { get; set; }

        public ReadOnlyTwoDimensionalCollection<Cell> Cells { get; } = new ReadOnlyTwoDimensionalCollection<Cell>(new Cell[0, 0]);
    }
}