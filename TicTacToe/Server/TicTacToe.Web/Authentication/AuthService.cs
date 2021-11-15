using System.Security.Claims;
using System.Text;
using TicTacToe.App.User;

namespace TicTacToe.Web.Authentication;

public class AuthService
{
    private readonly UserService _userService;

    public AuthService(UserService userService)
    {
        _userService = userService;
    }

    public Task<UserModel> GetUserAsync(string token)
    {
        return _userService.GetAsync(Encoding.UTF8.GetString(Convert.FromBase64String(token)));
    }

    public ClaimsPrincipal CreateClaimsPrincipal(UserModel user)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Name) }, TicTacToeAuthDefaults.AUTHENTICATION_SCHEME));
    }
}
