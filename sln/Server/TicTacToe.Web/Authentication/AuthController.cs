using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.App.User;
using TicTacToe.Core;

namespace TicTacToe.Web.Authentication;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthService _authService;

    public AuthController(UserService userService, AuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("SignIn")]
    public async Task<string> GetToken([FromForm] string userName, [FromForm] string password)
    {
        var user = await _userService.GetAsync(userName, password);

        if (user != null)
        {
            return _authService.GetJwtToken(user);
        }
        else
        {
            throw new TicTacToeException("Неправильные имя пользователя или пароль.");
        }
    }

    [AllowAnonymous]
    [HttpPost("SignUp")]
    public async Task SignUp([FromForm] string userName, [FromForm] string password)
    {
        await _userService.CreateAsync(userName, password);
    }
}