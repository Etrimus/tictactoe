using AutoMapper;
using TicTacToe.App;
using TicTacToe.Dal;
using TicTacToe.Web.Error;
using TicTacToe.Web.Game;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCors(setup => setup.AddDefaultPolicy(corsPolicyBuilder => corsPolicyBuilder
        // .AllowAnyOrigin()
        .WithOrigins("http://localhost:4200", "http://137.74.150.72:19883")
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader()))
    .AddSignalR();

builder.Services
    .AddDal()
    .AddApp();

builder.Services
    .AddControllers()
    .AddJsonOptions(opt => { opt.JsonSerializerOptions.PropertyNamingPolicy = null; });

var app = builder.Build();

app.UseExceptionHandler(applicationBuilder => { applicationBuilder.Run(ExceptionHandler.HandleAsync); });

app.Services.GetRequiredService<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();
//var executionPlan = app.Services.GetRequiredService<IMapper>().ConfigurationProvider.BuildExecutionPlan(typeof(ProfileEntry), typeof(ProfileModel));

app
    //.UseHttpsRedirection()
    .UseCors()
    .UseRouting()
    .UseEndpoints(endpoints => { endpoints.MapHub<GameHub>("/game-hub"); });

app.MapControllers();

app.Run();