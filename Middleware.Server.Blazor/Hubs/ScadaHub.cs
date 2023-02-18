using Microsoft.AspNetCore.SignalR;

namespace Middleware.Server.Blazor.Hubs
{
    public class ScadaHub:Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
