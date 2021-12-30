using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Demo_NotificationFromServerToClient_SignalR.Hubs
{
    public class NotificationMessageHub : Hub
    {
        public async Task SendMessage(string message, int durationMessage)
        {
            await Clients.Others.SendAsync("ReceiveMessage",message, durationMessage);
        }
    }
}
