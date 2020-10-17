using System;
using System.Linq;
using TicTacToe.Core.Models;

namespace TicTacToe.Core.Services
{
    internal class BoardManager
    {
        public static Cell[,] CreateCells(ushort gridSize)
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

            _inspectCells(board.Cells, out var winner, out var isNextTurnAvailable);

            board.Winner = winner;
            board.NextTurn = isNextTurnAvailable
                ? _getNextTurnCellType(board.NextTurn)
                : CellType.None;

            result = TurnResult.Success;
            return true;
        }

        private static CellType _getNextTurnCellType(CellType currentTurn)
        {
            return currentTurn switch
            {
                CellType.Cross => CellType.Zero,
                _ => CellType.Cross
            };
        }

        private static void _inspectCells(ReadOnlyTwoDimensionalCollection<Cell> cells, out CellType winner, out bool isNextTurnAvailable)
        {
            winner = CellType.None;
            isNextTurnAvailable = false;

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
                        isNextTurnAvailable = true;
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

            isNextTurnAvailable = isNextTurnAvailable && winner == CellType.None;
        }
    }
}