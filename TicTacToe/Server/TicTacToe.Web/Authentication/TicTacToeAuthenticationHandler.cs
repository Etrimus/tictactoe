using System.Security.Claims;
using System.Text.Encodings.Web;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using TicTacToe.Web.Game;

namespace TicTacToe.Web.Authentication;

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
        if (!Request.Headers.TryGetValue(TicTacToeAuthDefaults.AUTH_HEADER, out var header))
        {
            return AuthenticateResult.Fail($"http-заголовок '{TicTacToeAuthDefaults.AUTH_HEADER}' отсутствует в запросе.");
        }

        if (!Guid.TryParse(header[0], out var playerId))
        {
            return AuthenticateResult.Fail($"Неправильный http-заголовок '{TicTacToeAuthDefaults.AUTH_HEADER}' в запросе.");
        }

        if (!Context.TryGetGameId(out var gameId))
        {
            throw new Exception($"Нет правильного '{InjectGameIdMiddleware.GAME_ID_NAME}' в {nameof(Context)}.{nameof(Context.Items)}.");
        }

        if (await _authService.IsValidUserAsync(gameId, playerId))
        {
            return AuthenticateResult.Success(new AuthenticationTicket(_authService.CreateClaimsPrincipal(playerId), TicTacToeAuthDefaults.AUTHENTICATION_SCHEME));
        }

        return AuthenticateResult.Fail($"Неправильный {nameof(gameId)} или идентификатор игрока.");
    }

    public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
    {
        var playerId = user.FindFirstValue(ClaimTypes.Name);
        if (playerId == null)
        {
            throw new ArgumentException($"claim '{ClaimTypes.Name}' отсутствует в {nameof(ClaimsPrincipal)} {nameof(user)}.");
        }

        Context.Response.Headers.Add(TicTacToeAuthDefaults.AUTH_HEADER, playerId);

        return Task.CompletedTask;
    }

    public Task SignOutAsync(AuthenticationProperties properties)
    {
        throw new NotSupportedException();
    }
}
