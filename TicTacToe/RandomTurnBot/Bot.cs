using System;
using System.Linq;
using Core;
using Core.Models;

namespace RandomTurnBot
{
    public static class Bot
    {
        public static ushort Turn(Cell[,] cells)
        {
            var potentialResults = cells.Cast<Cell>().Where(x => x.State == CellType.None).ToArray();
            return (ushort)(potentialResults[new Random(DateTime.UtcNow.Millisecond).Next(0, potentialResults.Length - 1)].Number + 1);
        }
    }
}