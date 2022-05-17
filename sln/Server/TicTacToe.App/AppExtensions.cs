using Microsoft.Extensions.DependencyInjection;
using TicTacToe.App.Game;
using TicTacToe.Core;

namespace TicTacToe.App;

public static class AppExtensions
{
    public static IServiceCollection AddApp(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddTicTacToeCore()
            .AddAutoMapper(typeof(AppExtensions))
            .AddScoped<GameService>();
    }
}