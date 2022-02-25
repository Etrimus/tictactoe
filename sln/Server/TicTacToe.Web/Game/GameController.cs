using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TicTacToe.App.Game;
using TicTacToe.Core;

namespace TicTacToe.Web.Game;

[ApiController]
[Route("[controller]")]
public class GameController: ControllerBase
{
    private readonly GameService _gameService;
    private readonly IHubContext<GameHub> _hubContext;

    public GameController(GameService gameService, IHubContext<GameHub> hubContext)
    {
        _gameService = gameService;
        _hubContext = hubContext;
    }

    //[HttpGet("all")]
    //public Task<GameModel[]> GetAll()
    //{
    //    return _gameService.GetAllAsync();
    //}

    [HttpPost("id")]
    public Task<GameModel[]> GetById([FromForm] Guid[] id)
    {
        return _gameService.GetAsync(id);
    }

    [HttpPost("playerId")]
    public Task<GameModel[]> GetByPlayerId([FromForm] Guid playerId)
    {
        return _gameService.GetByPlayerIdAsync(playerId);
    }

    [HttpGet("free")]
    public Task<GameModel[]> GetFree()
    {
        return _gameService.GetFreeAsync();
    }

    [HttpGet("{id:guid}")]
    public Task<GameModel> Get([FromRoute] Guid id)
    {
        return _gameService.GetAsync(id, true);
    }

    [HttpPost]
    public Task<Guid> Add()
    {
        return _gameService.CreateNewAsync().ContinueWith(x => x.Result.Id);
    }

    [HttpPut("{id:guid}/crossPlayer")]
    public Task SetCrossPlayer([FromRoute] Guid id, [FromForm] Guid playerId)
    {
        return _gameService.SetPlayerAsync(id, playerId, CellType.Cross);
    }

    [HttpPut("{id:guid}/zeroPlayer")]
    public Task SetZeroPlayer([FromRoute] Guid id, [FromForm] Guid playerId)
    {
        return _gameService.SetPlayerAsync(id, playerId, CellType.Zero);
    }

    [HttpPut("{id:guid}/turn")]
    public async Task Turn([FromRoute] Guid id, [FromForm] Guid playerId, [FromForm] ushort? cellNumber)
    {
        await _gameService.MakeTurnAsync(id, playerId, cellNumber);
        await _hubContext.Clients.All.SendAsync("turn", id);
    }
}