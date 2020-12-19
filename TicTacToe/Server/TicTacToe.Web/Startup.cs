using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using TicTacToe.App;
using TicTacToe.Dal;
using TicTacToe.Web.Authentication;
using TicTacToe.Web.Error;
using TicTacToe.Web.Game;

namespace TicTacToe.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();

            services
                .AddTransient<InjectGameIdMiddleware>()
                .AddDal()
                .AddApp()
                .AddTicTacToeAuthentication();

            services.AddControllers();
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            {
                app.UseExceptionHandler(builder => { builder.Run(ExceptionHandler.HandleAsync); });
            }

            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            //var executionPlan = mapper.ConfigurationProvider.BuildExecutionPlan(typeof(GameEntity), typeof(Game));

            app
                .UseHttpsRedirection()
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider($"{env.ContentRootPath}/wwwroot/tic-tac-toe/dist/tic-tac-toe")
                })
                .UseRouting();

            app
                .UseMiddleware<InjectGameIdMiddleware>();

            app
                .UseAuthentication()
                .UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}