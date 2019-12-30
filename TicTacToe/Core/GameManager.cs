using System;
using System.Drawing;
using System.Linq;
using Core.Models;

namespace Core
{
    public class GameManager
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

            result.Player1 = new Player(result);
            result.Player2 = new Player(result);

            result.Player1.State = result.Player2.State = PlayerState.ChoiceType;

            return result;
        }

        public static void AdjustPlayerTypes(Board board, CellType player1, CellType player2)
        {
            var cellTypes = new[] { player1, player2 };

            var validTypes = cellTypes.Count(x => x == CellType.Zero) == 1 && cellTypes.Count(x => x == CellType.Cross) == 1;
            if (!validTypes)
            {
                throw new TicTacToeException("Один игрок должен выбрать игру крестиками, другой - ноликами.");
            }

            board.Player1.CellType = player1;
            board.Player1.State = _getStartedState(player1);

            board.Player2.CellType = player2;
            board.Player2.State = _getStartedState(player2);
        }

        public static void Turn(Player player, Point point)
        {
            if (player.State != PlayerState.WaitingForTurn)
            {
                throw new TicTacToeException($"Только игрок со статусом ${PlayerState.WaitingForTurn} может совершать ход.");
            }

            var cell = _findCell(player.Board.Cells, point);

            if (cell.State != CellType.Empty)
            {
                throw new TicTacToeException($"Выбранная для хода ячейка {point.X}:{point.Y} уже занята.");
            }

            cell.State = player.CellType;

            player.State = PlayerState.Waiting;
            _getAnotherPlayer(player).State = PlayerState.WaitingForTurn;
        }

        private static PlayerState _getStartedState(CellType cellType)
        {
            return cellType switch
            {
                CellType.Zero => PlayerState.Waiting,
                CellType.Cross => PlayerState.WaitingForTurn,
                _ => throw new ArgumentOutOfRangeException(nameof(cellType), cellType, null)
            };
        }

        private static Cell _findCell(Cell[,] cells, Point point)
        {
            return cells[point.X, point.Y];
        }

        private static Player _getAnotherPlayer(Player player)
        {
            if (ReferenceEquals(player, player.Board.Player1))
            {
                return player.Board.Player2;
            }

            if (ReferenceEquals(player, player.Board.Player2))
            {
                return player.Board.Player1;
            }

            throw new TicTacToeException("В текущей игре не найден указанный игрок.");
        }

        private void _checkCells(Cell[,] cells)
        {
            foreach (var cell in cells)
            { }
        }
    }
}