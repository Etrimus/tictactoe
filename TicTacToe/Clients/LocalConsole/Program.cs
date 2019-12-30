using System;
using System.Linq;
using Core;
using Core.Models;

namespace LocalConsole
{
    class Program
    {
        private const ushort BoardSize = 3;

        private static void Main()
        {
            while (true)
            {
                var board = GameManager.NewGame(BoardSize);

                Console.WriteLine("Вводите координаты хода в формате двух чисел через пробел.\n");

                while (board.Winner == CellType.Empty)
                {
                    _printBoard(board);
                    _printHeader(board);

                    ushort[] coords;
                    try
                    {
                        coords = Console.ReadLine()
                            .Trim()
                            .Split(' ')
                            .Select(x => Convert.ToUInt16(x))
                            .ToArray();

                        if (coords.Length != 2)
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Не удалось распознать введенные координаты хода.\n");
                        Console.ResetColor();

                        continue;
                    }

                    try
                    {
                        GameManager.Turn(board, new Point((ushort) (coords[0] - 1), (ushort) (coords[1] - 1)));
                    }
                    catch (TicTacToeException e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                        Console.ResetColor();
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