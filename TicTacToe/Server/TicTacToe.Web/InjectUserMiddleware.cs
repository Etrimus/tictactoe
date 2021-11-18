using TicTacToe.App.User;

namespace TicTacToe.Web
{
    internal class InjectUserMiddleware : IMiddleware
    {
        public const string USER_ITEM_KEY = "user";

        private readonly UserService _userService;

        public InjectUserMiddleware(UserService userService)
        {
            _userService = userService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.User.Identity.Name != null)
            {
                context.Items.Add(USER_ITEM_KEY, await _userService.GetAsync(context.User.Identity.Name, true));
            }

            await next(context);
        }
    }
}