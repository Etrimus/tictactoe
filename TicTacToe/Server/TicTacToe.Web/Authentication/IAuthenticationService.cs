using System;
using System.Threading.Tasks;

namespace TicTacToe.Web.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> IsValidUserAsync(Guid gameId, Guid playerId);
    }
}