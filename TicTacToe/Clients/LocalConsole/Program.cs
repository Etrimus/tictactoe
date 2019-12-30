using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Models;

namespace LocalConsole
{
    class Program
    {
        private static ushort _boardSize;

        static void Main(string[] args)
        {
            Console.WriteLine($"Введите размерность игрового поля в виде числа от {ushort.MinValue} до {ushort.MaxValue}...");
            _boardSize = Convert.ToUInt16(Console.ReadLine());

            var board = GameManager.NewGame(_boardSize);
            GameManager.AdjustPlayerTypes(board, CellType.Zero, CellType.Cross);

            _printBoard(board);

            Console.ReadLine();
        }

        private static void _printBoard(Board board)
        {
            Console.WriteLine();
            Console.WriteLine($"Игрок 1: {_cellTypeToString(board.Player1.CellType)}, {board.Player1.State}");
            Console.WriteLine($"Игрок 2: {_cellTypeToString(board.Player2.CellType)}, {board.Player2.State}");
            Console.WriteLine();

            for (var i = 0; i < _boardSize; i++)
            {
                for (var j = 0; j < _boardSize; j++)
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