using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace TicTacToe.Web.Error
{
    public static class ExceptionHandler
    {
        public static async Task HandleAsync(HttpContext context)
        {
            var exception = context.Features.Get<IExceptionHandlerPathFeature>();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ErrorDto {Message = exception.Error.Message});
        }
    }
}