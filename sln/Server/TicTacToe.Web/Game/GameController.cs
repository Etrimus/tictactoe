using Microsoft.AspNetCore.Mvc;
using TicTacToe.App.Game;
using TicTacToe.Core;

namespace TicTacToe.Web.Game;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly GameService _gameService;

    public GameController(GameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("all")]
    public Task<GameModel[]> GetAll()
    {
        return _gameService.GetAllAsync();
    }

    [HttpGet("free")]
    public Task<GameModel[]> GetFree()
    {
        return _gameService.GetFreeAsync();
    }

    [HttpGet("{id}")]
    public Task<GameModel> Get([FromRoute] Guid id)
    {
        return _gameService.GetAsync(id, true);
    }

    [HttpPost]
    public Task<Guid> Add()
    {
        return _gameService.CreateNewAsync().ContinueWith(x => x.Result.Id);
    }

    [HttpPut("{id}/crossPlayer")]
    public Task<Guid> SetCrossPlayer([FromRoute] Guid id)
    {
        return _gameService.SetPlayerAsync(id, CellType.Cross);
    }

    [HttpPut("{id}/zeroPlayer")]
    public Task<Guid> SetZeroPlayer([FromRoute] Guid id)
    {
        return _gameService.SetPlayerAsync(id, CellType.Zero);
    }

    [HttpPut("{id}/turn")]
    public Task Turn([FromRoute] Guid id, [FromForm] Guid playerId, [FromForm] ushort? cellNumber)
    {
        return _gameService.MakeTurnAsync(id, playerId, cellNumber);
    }
}