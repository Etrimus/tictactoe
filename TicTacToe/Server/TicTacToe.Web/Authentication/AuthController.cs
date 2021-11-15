using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.App.User;

namespace TicTacToe.Web.Authentication;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthService _authService;
    private readonly IAuthenticationService _authenticationService;

    public AuthController(UserService userService, AuthService authService, IAuthenticationService authenticationService)
    {
        _userService = userService;
        _authService = authService;
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [Route("")]
    public async Task<ActionResult> SignIn([FromForm] string userName, [FromForm] string password)
    {
        var user = await _userService.GetAsync(userName, password);

        if (user != null)
        {
            await _authenticationService.SignInAsync(HttpContext, TicTacToeAuthDefaults.AUTHENTICATION_SCHEME, _authService.CreateClaimsPrincipal(user), null);

            return Ok();
        }
        else
        {
            return Unauthorized();
        }
    }
}