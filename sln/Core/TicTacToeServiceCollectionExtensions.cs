using Microsoft.Extensions.DependencyInjection;

namespace TicTacToe.Core
{
    public static class TicTacToeServiceCollectionExtensions
    {
        public static IServiceCollection AddTicTacToeCore(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IBoardManager, BoardManager>();
        }
    }
}