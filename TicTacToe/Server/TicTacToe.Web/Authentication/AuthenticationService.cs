using System;
using System.Threading.Tasks;

namespace TicTacToe.Web.Authentication
{
    internal class AuthenticationService : IAuthenticationService
    {
        public Task<bool> IsValidUserAsync(Guid gameId, Guid playerId)
        {
            return Task.FromResult(true);
        }
    }
}