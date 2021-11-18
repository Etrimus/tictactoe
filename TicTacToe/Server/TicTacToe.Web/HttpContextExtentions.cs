using TicTacToe.App.User;

namespace TicTacToe.Web;

public static class HttpContextExtentions
{
    public static UserModel GetUser(this HttpContext context)
    {
        return (UserModel)context.Items[InjectUserMiddleware.USER_ITEM_KEY];
    }
}