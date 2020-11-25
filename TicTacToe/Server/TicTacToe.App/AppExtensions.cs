﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.App.Games;

namespace TicTacToe.App
{
    public static class AppExtensions
    {
        public static IServiceCollection AddApp(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddAutoMapper(typeof(AppExtensions))
                .AddScoped<GameService>();
        }
    }
}