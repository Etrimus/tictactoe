using Microsoft.AspNetCore.Diagnostics;

namespace TicTacToe.Web.Error
{
    public static class ExceptionHandler
    {
        public static async Task HandleAsync(HttpContext context)
        {
            var exception = context.Features.Get<IExceptionHandlerPathFeature>();
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorDto { Message = exception.Error.Message });
        }
    }
}