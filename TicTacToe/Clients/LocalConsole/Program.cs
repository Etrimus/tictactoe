using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core;
using Core.Models;
using RandomTurnBot;

namespace LocalConsole
{
    class Program
    {
        private const ushort BoardSize = 3;

        private static void Main()
        {
            while (true)
            {
                Board board;
                Dictionary<CellType, Func<Board, Point>> players;

                try
                {
                    players = _setPlayers();

                    Console.WriteLine("\nВводите координаты хода в формате двух чисел через пробел.\n");
                    board = GameManager.NewGame(BoardSize);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                while (board.Winner == CellType.Empty)
                {
                    try
                    {
                        _printBoard(board);
                        _printHeader(board);

                        GameManager.Turn(board, players[board.NextTurn](board));
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                        Console.ResetColor();
                        continue;
                    }
                    finally
                    {
                        Console.WriteLine();
                    }
                }

                _printBoard(board);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Победили {_cellTypeToString(board.Winner)}.");
                Console.ResetColor();

                Console.ReadLine();
            }
        }

        private static Dictionary<CellType, Func<Board, Point>> _setPlayers()
        {
            var result = new Dictionary<CellType, Func<Board, Point>>
            {
                {CellType.Zero, null},
                {CellType.Cross, null}
            };

            Console.WriteLine(
                $"\nВыберите, что вы будете ставить. {_cellTypeToString(CellType.Cross)} начинают игру первые:");
            Console.WriteLine($"{_cellTypeToString(CellType.Cross)} - {(int) CellType.Cross}");
            Console.WriteLine($"{_cellTypeToString(CellType.Zero)} - {(int) CellType.Zero}");

            var selectedType = (CellType) Convert.ToInt32(Console.ReadLine());
            if (selectedType == CellType.Empty || !Enum.IsDefined(typeof(CellType), selectedType))
            {
                throw new ArgumentException("Введенное значение некорректно.");
            }


            result[selectedType] = _getPlayerTurn;
            result[result.First(x => x.Value == null).Key] = _getBotTurn;

            return result;
        }

        private static Point _getBotTurn(Board board)
        {
            Thread.Sleep(2000);
            return Bot.Turn(board.Cells);
        }

        private static Point _getPlayerTurn(Board board)
        {
            var coords = Console.ReadLine()
                .Trim()
                .Split(' ')
                .Select(x => Convert.ToUInt16(x))
                .ToArray();

            return new Point((ushort) (coords[0] - 1), (ushort) (coords[1] - 1));
        }

        private static void _printHeader(Board board)
        {
            Console.WriteLine($"Ходят {_cellTypeToString(board.NextTurn)}.");
        }

        private static void _printBoard(Board board)
        {
            for (var i = 0; i < BoardSize; i++)
            {
                for (var j = 0; j < BoardSize; j++)
                {
                    Console.Write($"[{_cellTypeToSymbol(board.Cells[i, j].State)}]");
                }

                Console.WriteLine();
            }
        }

        private static string _cellTypeToSymbol(CellType cellType)
        {
            switch (cellType)
            {
                case CellType.Empty:
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
                case CellType.Empty:
                    return "Не выбрано";
                case CellType.Zero:
                    return "Нолики";
                case CellType.Cross:
                    return "Крестики";
                default:
                    throw new ArgumentOutOfRangeException(nameof(cellType), cellType, null);
            }
        }
    }
}