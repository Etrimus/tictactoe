using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace TicTacToe.Web.Authentication
{
    internal class TicTacToeAuthenticationHandler : AuthenticationHandler<TicTacToeAutSchemeOptions>
    {
        private const string AUTH_HEADER = "TicTacToeAuthorization";
        private const string GAME_ID_CLAIM_NAME = "GameId";

        private readonly IAuthenticationService _authService;

        public TicTacToeAuthenticationHandler(
            [NotNull] IOptionsMonitor<TicTacToeAutSchemeOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock,
            IAuthenticationService authService
        ) : base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //if (!Request.Headers.ContainsKey(AUTH_HEADER))
            //{
            //    return AuthenticateResult.NoResult();
            //}

            var gameId = Guid.NewGuid();
            var playerId = Guid.NewGuid();

            if (await _authService.IsValidUserAsync(gameId, playerId))
            {
                return AuthenticateResult.Success(
                    new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, gameId.ToString()),
                        new Claim(GAME_ID_CLAIM_NAME, gameId.ToString())
                    })), Scheme.Name));
            }

            return AuthenticateResult.Fail("Invalid game id or player id.");
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers[HeaderNames.WWWAuthenticate] = TicTacToeAuthDefaults.AuthenticationScheme;
            return base.HandleChallengeAsync(properties);
        }

        //protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        //{
        //    return base.HandleForbiddenAsync(properties);
        //}
    }
}