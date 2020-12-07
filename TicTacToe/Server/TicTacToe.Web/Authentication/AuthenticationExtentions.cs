using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TicTacToe.Web.Authentication
{
    internal static class AuthenticationExtentions
    {
        public static IServiceCollection AddTicTacToeAuthentication(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddAuthentication(TicTacToeAuthDefaults.AuthenticationScheme)
                .AddScheme<TicTacToeAutSchemeOptions, TicTacToeAuthenticationHandler>(TicTacToeAuthDefaults.AuthenticationScheme, opt => { });

            return serviceCollection
                .AddSingleton<IPostConfigureOptions<TicTacToeAutSchemeOptions>, TicTacToePostConfigureOptions>()
                .AddScoped<IAuthenticationService, AuthenticationService>();
        }
    }
}