using Microsoft.AspNetCore.SignalR;

namespace BE.Hubs;

public class MessageHub : Hub
{
    public async Task SendMessage(int receiverId, string content)
    {
        await Clients.User(receiverId.ToString()).SendAsync("messageReceived", content);
    }
}
