using System;
using Core.Models;

namespace Core
{
    public static class GameManager
    {
        public static Board NewGame(ushort gridSize)
        {
            var result = new Board(gridSize);

            for (var i = 0; i < gridSize; i++)
            {
                for (var j = 0; j < gridSize; j++)
                {
                    result.Cells[i, j] = new Cell();
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
            board.NextTurn = _getNextTurn(board.NextTurn);
        }

        private static Cell _findCell(Cell[,] cells, Point point)
        {
            try
            {
                return cells[point.X, point.Y];
            }
            catch (IndexOutOfRangeException e)
            {
                throw new TicTacToeException($"Ячейка с координатами {point.X + 1}:{point.Y + 1} не найдена на игровом поле.", e);
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

        private static void _checkCells(Cell[,] cells)
        {
            foreach (var cell in cells)
            { }
        }
    }
}