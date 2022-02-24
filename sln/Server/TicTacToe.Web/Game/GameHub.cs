using Microsoft.AspNetCore.SignalR;

namespace TicTacToe.Web.Game;

public class GameHub : Hub
{
    public Task Turn(Guid gameId)
    {
        return Clients.All.SendAsync("turn", gameId);
    }
}