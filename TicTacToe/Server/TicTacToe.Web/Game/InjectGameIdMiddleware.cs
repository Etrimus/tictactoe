using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TicTacToe.Web.Game
{
    public class InjectGameIdMiddleware : IMiddleware
    {
        public const string GAME_ID_NAME = "GameId";

        private const string GAME_CONTROLLER_TYPE_NAME = nameof(GameController);
        private readonly Regex _regex = new("(.*)(Controller)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var match = _regex.Match(GAME_CONTROLLER_TYPE_NAME);
            if (match.Success && context.Request.Path.Value != null)
            {
                var gameRouteToken = match.Groups[1].Value;
                var splittedPath = context.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries);

                if (splittedPath.Length > 1
                    && splittedPath[0].Equals(gameRouteToken, StringComparison.InvariantCultureIgnoreCase)
                    && Guid.TryParse(splittedPath[1], out var gameGuid))
                {
                    context.Items.Add(GAME_ID_NAME, gameGuid);
                }
            }

            await next(context);
        }
    }
}