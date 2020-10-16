using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using TicTacToe.Core;
using TicTacToe.Core.Models;

namespace TicTacToe.ClientNative.ConsoleClient
{
    public static class Program
    {
        private const ushort BOARD_SIZE = 3;

        private static void Main()
        {
            while (true)
            {
                Board board;
                Dictionary<CellType, Func<ReadOnlyTwoDimensionalCollection<Cell>, ushort>> players;

                try
                {
                    players = _setPlayers();

                    Console.WriteLine($"{Environment.NewLine}Вводите номер свободной ячейки для совершения хода.{Environment.NewLine}");

                    board = new Board(BOARD_SIZE);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                while (board.NextTurn != CellType.None)
                {
                    _printBoard(board);
                    _printHeader(board);

                    var playerTurnCellNumber = players[board.NextTurn].Invoke(board.Cells) - 1;

                    if (!board.TryTurn((ushort)playerTurnCellNumber, out var result))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(result);
                        Console.ResetColor();
                        continue;
                    }

                    Console.WriteLine();
                }

                _printBoard(board);

                Console.ForegroundColor = _cellTypeToColor(board.Winner);
                Console.WriteLine(board.Winner != CellType.None ? $"Победили {_cellTypeToString(board.Winner)}." : "Ничья.");
                Console.ResetColor();
                Console.ReadLine();
            }

            // ReSharper disable once FunctionNeverReturns
        }

        private static Dictionary<CellType, Func<ReadOnlyTwoDimensionalCollection<Cell>, ushort>> _setPlayers()
        {
            var result = new Dictionary<CellType, Func<ReadOnlyTwoDimensionalCollection<Cell>, ushort>>
            {
                { CellType.Zero, null },
                { CellType.Cross, null }
            };

            Console.WriteLine($"{Environment.NewLine}Выберите, что вы будете ставить. {_cellTypeToString(CellType.Cross)} начинают игру первые:");
            Console.WriteLine($"{_cellTypeToString(CellType.Cross)} - {(int)CellType.Cross}");
            Console.WriteLine($"{_cellTypeToString(CellType.Zero)} - {(int)CellType.Zero}");

            var selectedType = (CellType)Convert.ToInt32(Console.ReadLine());
            if (selectedType == CellType.None || !Enum.IsDefined(typeof(CellType), selectedType))
            {
                throw new ArgumentException("Введенное значение некорректно.");
            }

            result[selectedType] = _getPlayerTurn;
            result[result.First(x => x.Value == null).Key] = _getBotTurn;

            return result;
        }

        private static ushort _getBotTurn(ReadOnlyTwoDimensionalCollection<Cell> cells)
        {
            Thread.Sleep(1000);
            var result = Bot.RandomTurn.Bot.Turn(cells);
            Console.WriteLine(result);
            return result;
        }

        private static ushort _getPlayerTurn(ReadOnlyTwoDimensionalCollection<Cell> cells)
        {
            var input = Console.ReadLine();

            if (input == null)
            {
                throw new ArgumentException("Введите номер ячейки.");
            }

            var match = Regex.Match(input.Trim(), "^\\d+$");

            if (!match.Success)
            {
                throw new ArgumentException("Введите номер ячейки в корректном формате.");
            }

            return Convert.ToUInt16(match.Groups[0].Value);
        }

        private static void _printHeader(Board board)
        {
            Console.Write("Ходят ");
            Console.ForegroundColor = _cellTypeToColor(board.NextTurn);
            Console.Write($"{_cellTypeToString(board.NextTurn)}{Environment.NewLine}");
            Console.ResetColor();
        }

        private static void _printBoard(Board board)
        {
            for (var x = 0; x < BOARD_SIZE; x++)
            {
                for (var y = 0; y < BOARD_SIZE; y++)
                {
                    _getCellText(board.Cells[x, y], out var text, out var color);

                    Console.Write("[");
                    Console.ForegroundColor = color;
                    Console.Write(text);
                    Console.ResetColor();
                    Console.Write("]");
                }

                Console.WriteLine();
            }
        }

        private static void _getCellText(Cell cell, out string cellText, out ConsoleColor color)
        {
            switch (cell.State)
            {
                case CellType.None:
                    cellText = (cell.Number + 1).ToString();
                    color = ConsoleColor.DarkGray;
                    return;
                case CellType.Zero:
                    cellText = _cellTypeToSymbol(cell.State);
                    color = ConsoleColor.Cyan;
                    return;
                case CellType.Cross:
                    cellText = _cellTypeToSymbol(cell.State);
                    color = ConsoleColor.Yellow;
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string _cellTypeToSymbol(CellType cellType)
        {
            switch (cellType)
            {
                case CellType.None:
                    return " ";
                case CellType.Zero:
                    return "o";
                case CellType.Cross:
                    return "x";
                default:
                    throw new ArgumentOutOfRangeException(nameof(cellType), cellType, null);
            }
        }

        private static string _cellTypeToString(CellType cellType)
        {
            switch (cellType)
            {
                case CellType.None:
                    return "Не выбрано";
                case CellType.Zero:
                    return "Нолики";
                case CellType.Cross:
                    return "Крестики";
                default:
                    throw new ArgumentOutOfRangeException(nameof(cellType), cellType, null);
            }
        }

        private static ConsoleColor _cellTypeToColor(CellType cellType)
        {
            switch (cellType)
            {
                case CellType.None:
                    return ConsoleColor.White;
                case CellType.Zero:
                    return ConsoleColor.Cyan;
                case CellType.Cross:
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}