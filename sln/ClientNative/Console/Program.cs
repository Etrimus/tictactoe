﻿using System.Text;
using System.Text.RegularExpressions;
using TicTacToe.Core;
using TicTacToe.Core.Models;
using TicTacToe.Core.Services;

namespace TicTacToe.ClientNative.Console;

public static class Program
{
    private const ushort BoardSize = 3;

    private static void Main()
    {
        System.Console.OutputEncoding = Encoding.UTF8;

        var boardManager = new BoardManager();

        while (true)
        {
            Board board;
            Dictionary<CellType, Func<ReadOnlyTwoDimensionalCollection<Cell>, ushort>> players;

            try
            {
                players = _setPlayers();

                System.Console.WriteLine($"{Environment.NewLine}Введите номер ячейки для хода.{Environment.NewLine}");

                board = boardManager.CreateBoard(BoardSize);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                continue;
            }

            while (board.NextTurn != CellType.None)
            {
                _printBoard(board);
                _printHeader(board);

                var playerTurnCellNumber = players[board.NextTurn].Invoke(board.Cells) - 1;

                var turnResult = boardManager.Turn(board, (ushort)playerTurnCellNumber);
                if (turnResult != TurnResult.Success)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(turnResult);
                    System.Console.ResetColor();
                    continue;
                }

                System.Console.WriteLine();
            }

            _printBoard(board);

            System.Console.ForegroundColor = _cellTypeToColor(board.Winner);
            System.Console.WriteLine(board.Winner != CellType.None ? $"Победили {_cellTypeToString(board.Winner)}." : "Ничья.");
            System.Console.ResetColor();
            System.Console.ReadLine();
        }

        // ReSharper disable once FunctionNeverReturns
    }

    private static Dictionary<CellType, Func<ReadOnlyTwoDimensionalCollection<Cell>, ushort>> _setPlayers()
    {
        var result = new Dictionary<CellType, Func<ReadOnlyTwoDimensionalCollection<Cell>, ushort>>
        {
            {CellType.Zero, null},
            {CellType.Cross, null}
        };

        System.Console.WriteLine($"{Environment.NewLine}Выберите, что вы будете ставить. {_cellTypeToString(CellType.Cross)} начинают игру первые:");
        System.Console.WriteLine($"{_cellTypeToString(CellType.Cross)} - {(int)CellType.Cross}");
        System.Console.WriteLine($"{_cellTypeToString(CellType.Zero)} - {(int)CellType.Zero}");

        var selectedType = (CellType)Convert.ToInt32(System.Console.ReadLine());
        if (selectedType == CellType.None || !Enum.IsDefined(typeof(CellType), selectedType))
        {
            throw new TicTacToeException("Введенное значение некорректно.");
        }

        result[selectedType] = _getPlayerTurn;
        result[result.First(x => x.Value == null).Key] = _getBotTurn;

        return result;
    }

    private static ushort _getBotTurn(ReadOnlyTwoDimensionalCollection<Cell> cells)
    {
        Thread.Sleep(1000);
        var result = Bot.RandomTurn.RandomTurnBot.Turn(cells);
        System.Console.WriteLine(result);
        return result;
    }

    private static ushort _getPlayerTurn(ReadOnlyTwoDimensionalCollection<Cell> cells)
    {
        var input = System.Console.ReadLine();

        if (input == null)
        {
            throw new TicTacToeException("Введите номер ячейки.");
        }

        var match = Regex.Match(input.Trim(), "^\\d+$");

        if (!match.Success)
        {
            throw new TicTacToeException("Введите номер ячейки в корректном формате.");
        }

        return Convert.ToUInt16(match.Groups[0].Value);
    }

    private static void _printHeader(Board board)
    {
        System.Console.Write("Ходят ");
        System.Console.ForegroundColor = _cellTypeToColor(board.NextTurn);
        System.Console.Write($"{_cellTypeToString(board.NextTurn)}{Environment.NewLine}");
        System.Console.ResetColor();
    }

    private static void _printBoard(Board board)
    {
        for (var x = 0; x < BoardSize; x++)
        {
            for (var y = 0; y < BoardSize; y++)
            {
                _getCellText(board.Cells[x, y], out var text, out var color);

                System.Console.Write("[");
                System.Console.ForegroundColor = color;
                System.Console.Write(text);
                System.Console.ResetColor();
                System.Console.Write("]");
            }

            System.Console.WriteLine();
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
                throw new ArgumentOutOfRangeException($"{nameof(cell)}.{nameof(cell.State)}");
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