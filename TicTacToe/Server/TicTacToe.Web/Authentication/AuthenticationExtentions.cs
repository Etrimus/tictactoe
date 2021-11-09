using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace TicTacToe.Web.Authentication
{
    internal static class AuthenticationExtentions
    {
        public static IServiceCollection AddTicTacToeAuthentication(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddAuthentication(TicTacToeAuthDefaults.AUTHENTICATION_SCHEME)
                .AddScheme<AuthenticationSchemeOptions, TicTacToeAuthenticationHandler>(TicTacToeAuthDefaults.AUTHENTICATION_SCHEME, opt => { });

            return serviceCollection
                .AddScoped<AuthService, AuthService>();
        }
    }
}