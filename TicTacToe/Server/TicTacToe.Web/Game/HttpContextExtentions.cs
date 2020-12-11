using System;
using Microsoft.AspNetCore.Http;

namespace TicTacToe.Web.Game
{
    public static class HttpContextExtentions
    {
        public static bool TryGetGameId(this HttpContext context, out Guid gameId)
        {
            var result = context.Items.TryGetValue(InjectGameIdMiddleware.GameIdName, out var obj);

            if (!result || obj == null)
            {
                gameId = default;
                return result;
            }

            return Guid.TryParse(obj.ToString(), out gameId);
        }
    }
}