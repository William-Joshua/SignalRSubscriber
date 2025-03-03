using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Utils
{
    public class SubscriberChat:Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"客户端已连接: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"客户端断开连接: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string producer, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", producer, message);
        }
    }
}
