using Microsoft.AspNetCore.SignalR;

namespace TicTacToe.Web;

internal class GameHub: Hub
{
    public Task Turn(Guid gameId)
    {
        return Clients.All.SendAsync("turn");
    }
}