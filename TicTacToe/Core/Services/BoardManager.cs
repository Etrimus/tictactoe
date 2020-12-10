using System;
using System.Linq;
using TicTacToe.Core.Models;

namespace TicTacToe.Core.Services
{
    public static class BoardManager
    {
        public static Board CreateBoard(ushort gridSize)
        {
            return new Board(_createCells(gridSize));
        }

        public static Board CreateBoard(CellType nextTurn, CellType winner, CellType[,] cells)
        {
            return new Board(_createCells(cells), nextTurn, winner);
        }

        public static bool TryTurn(Board board, ushort cellNumber, out TurnResult result)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));

            var cell = board.Cells.FirstOrDefault(x => x.Number == cellNumber);

            if (cell == null)
            {
                result = TurnResult.CellDoesNotExist;
                return false;
            }

            if (cell.State != CellType.None)
            {
                result = TurnResult.CellIsAlreadyTaken;
                return false;
            }

            cell.State = board.NextTurn;

            _inspectCells(board.Cells, out var winner, out var isAnyFreeCells);

            board.Winner = winner;
            board.NextTurn = winner == CellType.None && isAnyFreeCells
                ? _getNextTurn(board.NextTurn)
                : CellType.None;

            result = TurnResult.Success;
            return true;
        }

        private static Cell[,] _createCells(ushort gridSize)
        {
            var result = new Cell[gridSize, gridSize];

            ushort cellNumber = 0;

            for (var i = 0; i < gridSize; i++)
            {
                for (var j = 0; j < gridSize; j++)
                {
                    result[i, j] = new Cell(cellNumber);
                    cellNumber++;
                }
            }

            return result;
        }

        private static Cell[,] _createCells(CellType[,] source)
        {
            var firstDimensionLength = source.GetLength(0);
            var secondDimensionLength = source.GetLength(1);

            if (firstDimensionLength != secondDimensionLength)
            {
                throw new ArgumentException($"Lengths of dimensions of array not equals.", nameof(source));
            }

            var result = new Cell[firstDimensionLength, secondDimensionLength];

            ushort cellNumber = 0;

            for (var i = 0; i < firstDimensionLength; i++)
            {
                for (var j = 0; j < secondDimensionLength; j++)
                {
                    result[i, j] = new Cell(cellNumber, source[i, j]);
                    cellNumber++;
                }
            }

            return result;
        }

        private static CellType _getNextTurn(CellType currentTurn)
        {
            return currentTurn switch
            {
                CellType.Cross => CellType.Zero,
                _ => CellType.Cross
            };
        }

        private static void _inspectCells(ReadOnlyTwoDimensionalCollection<Cell> cells, out CellType winner, out bool isAnyFreeCells)
        {
            winner = CellType.None;
            isAnyFreeCells = false;

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
                    if (cells[i, j].State == CellType.None)
                    {
                        isAnyFreeCells = true;
                    }

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

            static bool Predicate(CellType[] x) => x[0] != CellType.None && x.Distinct().Count() == 1;

            var horizontalWinner = horizontal.FirstOrDefault(Predicate);
            if (horizontalWinner != null)
            {
                winner = horizontalWinner[0];
            }

            var verticalWinner = vertical.FirstOrDefault(Predicate);
            if (verticalWinner != null)
            {
                winner = verticalWinner[0];
            }

            if (Predicate(diagonal1))
            {
                winner = diagonal1[0];
            }

            if (Predicate(diagonal2))
            {
                winner = diagonal2[0];
            }
        }
    }
}