namespace ChatApp.SignalR.Hubs;
using ChatApp.Api.Models;
using ChatApp.Api.Services.Repositories;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly IRepository<Group, Guid> groupRepository;

    public ChatHub(GroupsRepository groupRepository)
    {
        this.groupRepository = groupRepository;
    }

    public override async Task OnConnectedAsync()
    {
        if (await groupRepository.FindAsync(x => x.Name == "test") == null)
        {
            await groupRepository.AddAsync(new Group { Name = "test" });
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, "test");
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.Group("test").SendAsync("ReceiveMessage", user, message);
    }
}
