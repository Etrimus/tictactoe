using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.App.Game;
using TicTacToe.App.User;

namespace TicTacToe.Web.User;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly GameService _gameService;

    public UserController(GameService gameService, UserService userService)
    {
        _gameService = gameService;
    }

    [HttpGet("")]
    public Task<UserModel> GetUser()
    {
        return Task.FromResult(HttpContext.GetUser());
    }

    [HttpGet("games")]
    public Task<GameModel[]> GetMyGames()
    {
        return _gameService.GetByUserAsync(HttpContext.GetUser().Id);
    }
}