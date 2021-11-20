using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain;

namespace TicTacToe.Dal.Game;

public class GameRepository : Repository<GameEntity>
{
    public GameRepository(TicTacToeContext context) : base(context)
    { }

    protected override Func<DbSet<GameEntity>, IQueryable<GameEntity>> DbSetToQuery => (dbSet) => dbSet
    .Include(x => x.CrossPlayer)
    .Include(x => x.ZeroPlayer);
}