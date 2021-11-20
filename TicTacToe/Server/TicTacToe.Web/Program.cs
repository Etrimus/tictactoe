using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TicTacToe.App;
using TicTacToe.Dal;
using TicTacToe.Web;
using TicTacToe.Web.Authentication;
using TicTacToe.Web.Error;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCors(setup => setup.AddDefaultPolicy(builder => builder.AllowAnyOrigin()))
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
    .UseRouting()
    .UseCors();

app
    .UseAuthentication()
    .UseAuthorization();

app
    .UseMiddleware<InjectUserMiddleware>();

app.MapControllers();

app.Run();
