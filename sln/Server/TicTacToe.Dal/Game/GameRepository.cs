using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain;

namespace TicTacToe.Dal.Game;

public class GameRepository(TicTacToeContext context): Repository<GameEntity>(context)
{
    protected override Func<DbSet<GameEntity>, IQueryable<GameEntity>> DbSetToQuery => dbSet => dbSet;
}