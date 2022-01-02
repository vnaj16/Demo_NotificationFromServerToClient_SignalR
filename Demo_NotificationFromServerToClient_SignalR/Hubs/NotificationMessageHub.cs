using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_NotificationFromServerToClient_SignalR.Hubs
{
    public class NotificationMessageHub : Hub
    {
        public async Task SendMessage(string username, string message, int durationMessage, bool onlyToMyGroup)
        {//DE LOS GRUPOS POR ROLE
            var context = Context;

            if (onlyToMyGroup)
            {
                var groupName = Context.User.Claims
                    .Where(x => x.Type.Contains("role")).FirstOrDefault().Value;
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId + "-" + Context.UserIdentifier} has joined the group {groupName}.", 6);
                await Clients.Group(groupName).SendAsync("ReceiveMessage", message + $" ({context.UserIdentifier})", durationMessage);
            }
            else
            {
                if (username == "All")
                {
                    await Clients.Others.SendAsync("ReceiveMessage", message + $" ({context.UserIdentifier})", durationMessage);
                }
                else
                {
                    await Clients.User(username).SendAsync("ReceiveMessage", message + $" (From: {context.UserIdentifier})", durationMessage);
                }
            }
        }
    }
}
