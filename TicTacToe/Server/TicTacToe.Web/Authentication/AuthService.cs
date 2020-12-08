using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TicTacToe.Dal.Games;

namespace TicTacToe.Web.Authentication
{
    public class AuthService
    {
        private readonly GameRepository _gameRepository;

        public AuthService(GameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<bool> IsValidUserAsync(Guid gameId, Guid playerId)
        {
            var game = await _gameRepository.GetAsync(gameId);

            return game != null && (game.CrossId == playerId || game.ZeroId == playerId);
        }

        public ClaimsPrincipal CreateClaimsPrincipal(Guid gameId, Guid playerId)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(
                new[] {new Claim(ClaimTypes.Name, playerId.ToString()), new Claim(TicTacToeAuthDefaults.ClaimGameId, gameId.ToString())},
                TicTacToeAuthDefaults.AuthenticationScheme));
        }
    }
}