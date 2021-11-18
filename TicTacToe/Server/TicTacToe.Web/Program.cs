using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using TicTacToe.App;
using TicTacToe.Dal;
using TicTacToe.Web;
using TicTacToe.Web.Authentication;
using TicTacToe.Web.Error;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication();

builder.Services
    .AddScoped<AuthService>()
    .AddTransient<InjectUserMiddleware>()
    .AddDal()
    .AddApp();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,

            ValidateIssuer = true,
            ValidIssuer = JwtOptions.ISSUER,

            ValidateAudience = true,
            ValidAudience = JwtOptions.AUDIENCE,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = JwtOptions.GetSymmetricSecurityKey()
        };
    });

builder.Services
    .AddControllers();

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
    .UseAuthentication()
    .UseAuthorization();

app
    .UseMiddleware<InjectUserMiddleware>();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();
