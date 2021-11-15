using System.Text.RegularExpressions;

namespace TicTacToe.Web.Game;

public class InjectGameIdMiddleware : IMiddleware
{
    public const string GAME_ID_NAME = "GameId";

    private const string GAME_CONTROLLER_TYPE_NAME = nameof(GameController);
    private readonly Regex _regex = new($"{GAME_CONTROLLER_TYPE_NAME}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        //if (GAME_CONTROLLER_TYPE_NAME != $"{context.Request.RouteValues["controller"]}Controller")
        //{
        //    await next(context);
        //}

        
        //if(Guid.TryParse(context.Request.RouteValues.Values.ToArray()[2], out var gameId))

        //if (match.Success && context.Request.Path.Value != null)
        //{
        //    var gameRouteToken = match.Groups[1].Value;
        //    var splittedPath = context.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries);

        //    if (splittedPath.Length > 1
        //        && splittedPath[0].Equals(gameRouteToken, StringComparison.InvariantCultureIgnoreCase)
        //        && Guid.TryParse(splittedPath[1], out var gameGuid))
        //    {
        //        context.Items.Add(GAME_ID_NAME, gameGuid);
        //    }
        //}

        await next(context);
    }
}
