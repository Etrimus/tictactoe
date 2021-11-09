using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TicTacToe.Core;
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

            if (game == null)
            {
                throw new TicTacToeException("Игра с указанным Id не существует.");
            }


            if (!game.CrossId.HasValue && !game.ZeroId.HasValue)
            {
                throw new TicTacToeException("У игры не назначен ни игрок крестики, ни игрок нолики.");
            }

            return game.CrossId == playerId || game.ZeroId == playerId;
        }

        public ClaimsPrincipal CreateClaimsPrincipal(Guid gameId, Guid playerId)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, playerId.ToString()) }, TicTacToeAuthDefaults.AUTHENTICATION_SCHEME));
        }
    }
}