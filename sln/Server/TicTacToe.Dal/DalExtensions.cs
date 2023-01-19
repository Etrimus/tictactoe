using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TicTacToe.Dal;

public static class DalExtensions
{
    public static IServiceCollection AddDal(this IServiceCollection serviceCollection)
    {
        foreach (var type in _getRepositories())
        {
            serviceCollection.AddScoped(type);
        }

        return serviceCollection
            .AddDbContext<TicTacToeContext>(builder => builder
                .UseInMemoryDatabase("TicTacToeDb")
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());
    }

    private static IEnumerable<Type> _getRepositories()
    {
        return Assembly
            .GetAssembly(typeof(Repository))
            .GetTypes()
            .Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(Repository)))
            .ToArray();
    }
}