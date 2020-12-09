using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicTacToe.App;
using TicTacToe.Dal;
using TicTacToe.Web.Authentication;
using TicTacToe.Web.Games;

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseHttpsRedirection()
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