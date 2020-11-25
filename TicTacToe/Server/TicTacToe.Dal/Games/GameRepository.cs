using TicTacToe.Domain;

namespace TicTacToe.Dal.Games
{
    public class GameRepository : Repository<GameEntity>
    {
        public GameRepository(TicTacToeContext context) : base(context)
        { }
    }
}