using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TicTacToe.Web.Games;

namespace TicTacToe.Web.Authentication
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class TicTacToeAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>, IAuthenticationSignInHandler
    {
        private readonly AuthService _authService;

        public TicTacToeAuthenticationHandler(
            [NotNull] IOptionsMonitor<AuthenticationSchemeOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock,
            AuthService authService
        ) : base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(TicTacToeAuthDefaults.AuthHeader, out var header))
            {
                return AuthenticateResult.Fail($"No '{TicTacToeAuthDefaults.AuthHeader}' header was provided.");
            }

            if (!Guid.TryParse(header[0], out var playerId))
            {
                return AuthenticateResult.Fail($"Invalid '{TicTacToeAuthDefaults.AuthHeader}' header was provided.");
            }

            if (!Context.Items.TryGetValue(InjectGameIdMiddleware.GameIdName, out var gameId))
            {
                throw new Exception($"No '{InjectGameIdMiddleware.GameIdName}' item in {nameof(Context)}.{nameof(Context.Items)}.");
            }

            if (await _authService.IsValidUserAsync((Guid)gameId, playerId))
            {
                return AuthenticateResult.Success(new AuthenticationTicket(_authService.CreateClaimsPrincipal((Guid)gameId, playerId), TicTacToeAuthDefaults.AuthenticationScheme));
            }

            return AuthenticateResult.Fail("Invalid game id or player id.");
        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            var playerId = user.FindFirstValue(ClaimTypes.Name);
            if (playerId == null)
            {
                throw new ArgumentException($"No claim '{ClaimTypes.Name}' was presented in {nameof(user)}.");
            }

            var gameId = user.FindFirstValue(TicTacToeAuthDefaults.ClaimGameId);
            if (gameId == null)
            {
                throw new ArgumentException($"No claim '{TicTacToeAuthDefaults.ClaimGameId}' was presented in {nameof(user)}.");
            }

            Context.Response.Headers.Add(TicTacToeAuthDefaults.AuthHeader, $"{playerId};{gameId}");

            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            throw new NotSupportedException();
        }
    }
}