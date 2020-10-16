using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using TicTacToe.Core.Models;
using TicTacToe.Core.Services;
using Xunit;

namespace TicTacToe.Core.Tests.Unit
{
    public class BoardManagerTests
    {
        private readonly Fixture _fixture = new Fixture();

        private readonly BoardManager _boardManager = new BoardManager();

        [Fact]
        public void CreateCells_CorrectDimensionCount()
        {
            const int expected = 2;

            var cellsArray = _boardManager.CreateCells(0);

            cellsArray.Rank.Should().Be(expected);
        }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(10)]
        public void CreateCells_CorrectDimensionsSize(ushort expected)
        {
            var cellsArray = _boardManager.CreateCells(expected);

            cellsArray.GetLength(0).Should().Be(expected);
            cellsArray.GetLength(1).Should().Be(expected);
        }

        [Fact]
        public void CreateCells_CorrectCellState()
        {
            var cellsArray = _boardManager.CreateCells(3);

            using (new AssertionScope())
            {
                foreach (var cell in cellsArray)
                {
                    cell.State.Should().Be(CellType.None);
                }
            }
        }

        [Fact]
        public void CreateCells_CorrectCellNumbers()
        {
            const ushort gridSize = 3;

            var cellsArray = _boardManager.CreateCells(gridSize).Cast<Cell>().Select(x => x.Number).ToArray();

            cellsArray.Should().Contain(new ushort[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
        }

        [Fact]
        public void TryTurn_BoardIsNull_ThrowArgumentNullException()
        {
            _boardManager.Invoking(x => x.TryTurn(null, 0, out _))
                .Should()
                .Throw<ArgumentNullException>();
        }
    }
}