namespace TicTacToe.Web.Game;

public static class HttpContextExtentions
{
    public static bool TryGetGameId(this HttpContext context, out Guid gameId)
    {
        var result = context.Items.TryGetValue(InjectGameIdMiddleware.GAME_ID_NAME, out var obj);

        if (!result || obj == null)
        {
            gameId = default;
            return result;
        }

        return Guid.TryParse(obj.ToString(), out gameId);
    }
}
