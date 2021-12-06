using Microsoft.Extensions.DependencyInjection;
using TicTacToe.App.Game;
using TicTacToe.Core.Services;

namespace TicTacToe.App
{
    public static class AppExtensions
    {
        public static IServiceCollection AddApp(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddAutoMapper(typeof(AppExtensions))
                .AddScoped<BoardManager>()
                .AddScoped<GameService>();
        }
    }
}