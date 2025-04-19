using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TicTacToe.App.Game;
using TicTacToe.Core;

namespace TicTacToe.Web.Game;

[ApiController, Route("[controller]")]
public class GameController(GameService gameService, IHubContext<GameHub, IGameHubClient> hubContext): ControllerBase
{
    //[HttpGet("all")]
    //public Task<GameModel[]> GetAll()
    //{
    //    return _gameService.GetAllAsync();
    //}

    [HttpPost("id")]
    public Task<GameModel[]> GetById([FromForm] Guid[] id)
    {
        return gameService.GetAsync(id);
    }

    [HttpPost("playerId")]
    public Task<GameModel[]> GetByPlayerId([FromForm] Guid playerId)
    {
        return gameService.GetByPlayerIdAsync(playerId);
    }

    [HttpGet("free")]
    public Task<GameModel[]> GetFree()
    {
        return gameService.GetFreeAsync();
    }

    [HttpGet("{id:guid}")]
    public Task<GameModel> Get([FromRoute] Guid id)
    {
        return gameService.GetAsync(id, true);
    }

    [HttpPost]
    public async Task<Guid> Add()
    {
        var result = await gameService.CreateNewAsync();
        await hubContext.Clients.All.GameAdded(result.Id);
        return result.Id;
    }

    [HttpPut("{id:guid}/crossPlayer")]
    public async Task SetCrossPlayer([FromRoute] Guid id, [FromForm] Guid playerId)
    {
        await gameService.SetPlayerAsync(id, playerId, CellType.Cross);
        await hubContext.Clients.All.GameUpdated(id);
    }

    [HttpPut("{id:guid}/zeroPlayer")]
    public async Task SetZeroPlayer([FromRoute] Guid id, [FromForm] Guid playerId)
    {
        await gameService.SetPlayerAsync(id, playerId, CellType.Zero);
        await hubContext.Clients.All.GameUpdated(id);
    }

    [HttpPut("{id:guid}/turn")]
    public async Task Turn([FromRoute] Guid id, [FromForm] Guid playerId, [FromForm] ushort? cellNumber)
    {
        await gameService.MakeTurnAsync(id, playerId, cellNumber);
        await hubContext.Clients.All.GameUpdated(id);
    }
}