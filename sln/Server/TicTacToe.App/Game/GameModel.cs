using TicTacToe.Core.Models;

namespace TicTacToe.App.Game;

public class GameModel: ModelBase
{
    [Obsolete("For Automapper only.", true)]
    public GameModel()
    { }

    public GameModel(Board board)
    {
        Board = board;
    }

    public Board Board { get; set; }

    public Guid? CrossPlayerId { get; set; }

    public Guid? ZeroPlayerId { get; set; }
}