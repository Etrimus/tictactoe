using AutoMapper;
using TicTacToe.App;
using TicTacToe.Dal;
using TicTacToe.Web.Error;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCors(setup => setup.AddDefaultPolicy(builder => builder.AllowAnyOrigin()))
    .AddAuthentication();

builder.Services
    .AddDal()
    .AddApp();

builder.Services
    .AddControllers();

var app = builder.Build();

app.UseExceptionHandler(builder => { builder.Run(ExceptionHandler.HandleAsync); });

app.Services.GetRequiredService<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();
//var executionPlan = app.Services.GetRequiredService<IMapper>().ConfigurationProvider.BuildExecutionPlan(typeof(ProfileEntry), typeof(ProfileModel));

app
    .UseHttpsRedirection()
    .UseRouting()
    .UseCors();

app.MapControllers();

app.Run();