using System.Linq.Expressions;
using AutoMapper;

namespace TicTacToe.App;

internal static class AutoMapperExtensions
{
    public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination, TMember>(
        this IMappingExpression<TSource, TDestination> expression,
        Expression<Func<TDestination, TMember>> destinationMember)
    {
        return expression
            .ForMember(destinationMember, opt => opt.Ignore());
    }
}