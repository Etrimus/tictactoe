using System;
using NUnit.Framework;
using TicTacToe.Core.Services;

namespace TicTacToe.Tests;

[TestFixture]
public class BoardManagerTests
{
    private BoardManager _boardManager;

    [SetUp]
    public void Setup()
    {
        _boardManager = new BoardManager();
    }

    /// <summary>
    /// Ячейки созданной игровой доски должны представлять из себя 2-мерный массив.
    /// </summary>
    [Test]
    public void CreateBoard_Cells_Dimensions_Should_Equal_Two()
    {
        var cells = _boardManager.CreateBoard(2).Cells;

        Assert.That(() => cells.GetLength(2), Throws.Exception.TypeOf<IndexOutOfRangeException>());
    }
}