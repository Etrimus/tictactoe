using AutoMapper;
using TicTacToe.App;
using TicTacToe.Dal;
using TicTacToe.Web;
using TicTacToe.Web.Error;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCors(setup => setup.AddDefaultPolicy(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()))
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
    .UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin())
    .UseRouting()
    .UseEndpoints(endpoints => { endpoints.MapHub<GameHub>("/game-hub"); });

app.MapControllers();

app.Run();