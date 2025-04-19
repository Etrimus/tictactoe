using Microsoft.AspNetCore.SignalR;

namespace TicTacToe.Web.Game;

public class GameHub: Hub<IGameHubClient>
{
    public Task GameAdded(Guid addedGmeId)
    {
        return Clients.All.GameAdded(addedGmeId);
    }

    public Task GameUpdated(Guid gameId)
    {
        return Clients.All.GameUpdated(gameId);
    }
}

public interface IGameHubClient
{
    [HubMethodName("game-added")]
    Task GameAdded(Guid addedGameId);

    [HubMethodName("game-updated")]
    Task GameUpdated(Guid gameId);
}