using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain;

namespace TicTacToe.Dal.User;

public class UserRepository : Repository<UserEntity>
{
    public UserRepository(TicTacToeContext context) : base(context)
    { }

    public Task<UserEntity> GetAsync(string userName, string password)
    {
        return QueryAsync().FirstOrDefaultAsync(x => x.Name == userName && x.Password == password);
    }

    public Task<UserEntity> GetAsync(string userName)
    {
        return QueryAsync().FirstOrDefaultAsync(x => x.Name == userName);
    }
}