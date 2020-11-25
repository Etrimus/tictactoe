using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Dal.Games;

namespace TicTacToe.Dal
{
    public static class DalExtensions
    {
        public static IServiceCollection AddDal(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddDbContext<TicTacToeContext>()
                .AddScoped<GameRepository>();
        }
    }
}