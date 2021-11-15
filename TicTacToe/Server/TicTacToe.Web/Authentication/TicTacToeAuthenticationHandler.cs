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

        var user = await _authService.GetUserAsync(header[0]);
        if (user != null)
        {
            return AuthenticateResult.Success(new AuthenticationTicket(_authService.CreateClaimsPrincipal(user), TicTacToeAuthDefaults.AUTHENTICATION_SCHEME));
        }

        return AuthenticateResult.Fail($"Неправильный идентификатор игрока.");
    }

    public Task SignInAsync(ClaimsPrincipal userClaims, AuthenticationProperties properties)
    {
        return Task.CompletedTask;

        var userName = userClaims.FindFirstValue(ClaimTypes.Name);
        if (userName == null)
        {
            throw new ArgumentException($"Claim '{ClaimTypes.Name}' отсутствует в {nameof(ClaimsPrincipal)} {nameof(userClaims)}.");
        }

        Context.Response.Headers.Add(TicTacToeAuthDefaults.AUTH_HEADER, userName);

        return Task.CompletedTask;
    }

    public Task SignOutAsync(AuthenticationProperties properties)
    {
        throw new NotSupportedException();
    }
}
