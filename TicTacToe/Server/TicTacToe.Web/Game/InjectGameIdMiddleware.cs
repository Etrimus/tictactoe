using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TicTacToe.Web.Game
{
    public class InjectGameIdMiddleware : IMiddleware
    {
        public const string GameIdName = "GameId";

        private const string GameControllerTypeName = nameof(GameController);
        private readonly Regex _regex = new Regex("(.*)(Controller)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var match = _regex.Match(GameControllerTypeName);
            if (match.Success && context.Request.Path.Value != null)
            {
                var gameRouteToken = match.Groups[1].Value;
                var splittedPath = context.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries);

                if (splittedPath.Length > 1
                    && splittedPath[0].Equals(gameRouteToken, StringComparison.InvariantCultureIgnoreCase)
                    && Guid.TryParse(splittedPath[1], out var gameGuid))
                {
                    context.Items.Add(GameIdName, gameGuid);
                }
            }

            await next(context);
        }
    }
}