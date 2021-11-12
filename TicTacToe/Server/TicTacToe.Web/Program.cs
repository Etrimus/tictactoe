using AutoMapper;
using Microsoft.Extensions.FileProviders;
using TicTacToe.App;
using TicTacToe.Dal;
using TicTacToe.Web.Authentication;
using TicTacToe.Web.Error;
using TicTacToe.Web.Game;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication();

builder.Services
    .AddControllers();

builder.Services
    .AddTransient<InjectGameIdMiddleware>()
    .AddDal()
    .AddApp()
    .AddTicTacToeAuthentication();

var app = builder.Build();

app.UseExceptionHandler(builder => { builder.Run(ExceptionHandler.HandleAsync); });

app.Services.GetRequiredService<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();
//var executionPlan = app.Services.GetRequiredService<IMapper>().ConfigurationProvider.BuildExecutionPlan(typeof(ProfileEntry), typeof(ProfileModel));

app
    .UseHttpsRedirection()
    .UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider($"{app.Services.GetRequiredService<IWebHostEnvironment>().ContentRootPath}/wwwroot/tic-tac-toe/dist/tic-tac-toe")
    })
    .UseRouting();

app
    .UseMiddleware<InjectGameIdMiddleware>();

app
    .UseAuthentication()
    .UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();
