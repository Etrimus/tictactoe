using System;

namespace Core
{
    public class TicTacToeException : Exception
    {
        public TicTacToeException()
        { }

        public TicTacToeException(string message) : base(message)
        { }

        public TicTacToeException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}