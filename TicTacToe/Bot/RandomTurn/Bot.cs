using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.Core;
using TicTacToe.Core.Models;

namespace TicTacToe.Bot.RandomTurn
{
    public static class Bot
    {
        public static ushort Turn(IEnumerable<Cell> cells)
        {
            var potentialResults = cells.Where(x => x.State == CellType.None).ToArray();
            return (ushort)(potentialResults[new Random(DateTime.UtcNow.Millisecond).Next(0, potentialResults.Length - 1)].Number + 1);
        }
    }
}