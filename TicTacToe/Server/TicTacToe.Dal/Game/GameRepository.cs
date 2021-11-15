using TicTacToe.Domain;

namespace TicTacToe.Dal.Game;

public class GameRepository : Repository<GameEntity>
{
    public GameRepository(TicTacToeContext context) : base(context)
    { }
}