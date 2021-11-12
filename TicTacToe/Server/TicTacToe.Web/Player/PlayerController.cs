using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.App.Game;

namespace TicTacToe.Web.Player;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    private readonly GameService _gameService;

    public PlayerController(GameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("games")]
    [Authorize]
    public Task<GameModel[]> GetMyGames()
    {
        return _gameService.GetMyAsync(default);
    }
}