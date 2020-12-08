using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace TicTacToe.Web.Authentication
{
    internal static class AuthenticationExtentions
    {
        public static IServiceCollection AddTicTacToeAuthentication(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddAuthentication(TicTacToeAuthDefaults.AuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, TicTacToeAuthenticationHandler>(TicTacToeAuthDefaults.AuthenticationScheme, opt => { });

            return serviceCollection
                .AddScoped<AuthService, AuthService>();
        }
    }
}