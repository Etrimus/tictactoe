namespace TicTacToe.Core;

public class TicTacToeException: Exception
{
    public TicTacToeException(string message): base(message)
    { }

    public TicTacToeException(string message, Exception innerException): base(message, innerException)
    { }
}