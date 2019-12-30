using System;
using System.Linq;
using Core.Models;

namespace Core
{
    public static class GameManager
    {
        public static Board NewGame(ushort gridSize)
        {
            var result = new Board(gridSize);
            var num = 0;

            for (var i = 0; i < gridSize; i++)
            {
                for (var j = 0; j < gridSize; j++)
                {
                    result.Cells[i, j] = new Cell(num);
                    num++;
                }
            }

            return result;
        }

        public static void Turn(Board board, Point point)
        {
            var cell = _findCell(board.Cells, point);

            if (cell.State != CellType.Empty)
            {
                throw new TicTacToeException($"Выбранная для хода ячейка {point.X + 1}:{point.Y + 1} уже занята.");
            }

            cell.State = board.NextTurn;

            board.Winner = _findWinner(board.Cells);

            board.NextTurn = board.Winner == CellType.Empty
                ? _getNextTurn(board.NextTurn)
                : CellType.Empty;
        }

        private static Cell _findCell(Cell[,] cells, Point point)
        {
            try
            {
                return cells[point.X, point.Y];
            }
            catch (IndexOutOfRangeException e)
            {
                throw new TicTacToeException(
                    $"Ячейка с координатами {point.X + 1}:{point.Y + 1} не найдена на игровом поле.", e);
            }
        }

        private static CellType _getNextTurn(CellType currentTurn)
        {
            switch (currentTurn)
            {
                case CellType.Empty:
                case CellType.Zero:
                    return CellType.Cross;
                case CellType.Cross:
                    return CellType.Zero;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentTurn), currentTurn, null);
            }
        }

        private static CellType _findWinner(Cell[,] cells)
        {
            var size = cells.GetLength(0);

            var horizontal = new CellType[size][];
            var vertical = new CellType[size][];
            var diagonal1 = new CellType[size];
            var diagonal2 = new CellType[size];

            for (var i = 0; i < size; i++)
            {
                horizontal[i] = new CellType[size];

                for (var j = 0; j < size; j++)
                {
                    horizontal[i][j] = cells[i, j].State;

                    if (i == 0)
                    {
                        vertical[j] = new CellType[size];
                    }

                    vertical[j][i] = cells[i, j].State;
                }

                diagonal1[i] = cells[i, i].State;
                diagonal2[i] = cells[i, size - i - 1].State;
            }

            static bool Predicate(CellType[] x) => x[0] != CellType.Empty && x.Distinct().Count() == 1;

            var horizontalWinner = horizontal.FirstOrDefault(Predicate);
            if (horizontalWinner != null)
            {
                return horizontalWinner[0];
            }

            var verticalWinner = horizontal.FirstOrDefault(Predicate);
            if (verticalWinner != null)
            {
                return verticalWinner[0];
            }

            if (Predicate(diagonal1))
            {
                return diagonal1[0];
            }

            if (Predicate(diagonal2))
            {
                return diagonal2[0];
            }

            return CellType.Empty;
        }
    }
}