using System;
using AutoFixture;
using FluentAssertions;
using TicTacToe.Core.Services;
using Xunit;

namespace TicTacToe.Core.Tests
{
    public class BoardManagerTests
    {
        private readonly Fixture _fixture = new Fixture();

        private readonly BoardManager _boardManager = new BoardManager();

        [Fact]
        public void TryTurn_BoardIsNull_ThrowArgumentNullException()
        {
            _boardManager.Invoking(x => x.Turn(null, 0))
                .Should()
                .Throw<ArgumentNullException>();
        }
    }
}