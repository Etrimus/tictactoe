using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain;

namespace TicTacToe.Dal.User;

public class UserRepository : Repository<UserEntity>
{
    public UserRepository(TicTacToeContext context) : base(context)
    { }

    protected override Func<DbSet<UserEntity>, IQueryable<UserEntity>> DbSetToQuery => (dbSet) => dbSet;

    public Task<UserEntity> GetAsync(string userName, string password)
    {
        return Query().FirstOrDefaultAsync(x => x.Name == userName && x.Password == password);
    }

    public Task<UserEntity> GetAsync(string userName)
    {
        return Query().FirstOrDefaultAsync(x => x.Name == userName);
    }
}