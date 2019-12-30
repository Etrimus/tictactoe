using System;
using System.Collections.Generic;
using Core;
using Core.Models;

namespace RandomTurnBot
{
    public static class Bot
    {
        public static Point Turn(Cell[,] cells)
        {
            var size = cells.GetLength(0);

            var potentialResults = new List<Point>();

            for (ushort i = 0; i < size; i++)
            {
                for (ushort j = 0; j < size; j++)
                {
                    if (cells[i, j].State == CellType.Empty)
                    {
                        potentialResults.Add(new Point(i, j));
                    }
                }
            }

            return potentialResults[new Random(DateTime.UtcNow.Millisecond).Next(0, potentialResults.Count - 1)];
        }
    }
}