namespace ChatApp.Api.Hubs;

using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

public class PrivateChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string receiverConnectionId, string message)
    {
        await Clients.Client(receiverConnectionId).SendAsync(message);
    }
}
