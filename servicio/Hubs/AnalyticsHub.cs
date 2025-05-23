using Microsoft.AspNetCore.SignalR;

namespace servicio.Hubs
{
    public class AnalyticsHub : Hub
    {
        public async Task SendMessageToClients(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendUpdate(string update)
        {
            await Clients.All.SendAsync("ReceiveUpdate", update);
        }
    }
}
