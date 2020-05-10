using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial.Chat.Application.SignalR
{
    public class ChatHub : Hub
    {
        public static IDictionary<string, string> users = new Dictionary<string, string>();

        public async override Task OnConnectedAsync()
        {
            var email = Context.GetHttpContext().Request.Query["email"];

            if(users.Any(x => x.Value == email))
                await Groups.RemoveFromGroupAsync(users.FirstOrDefault(x => x.Value == email).Key, users.FirstOrDefault(x => x.Value == email).Value);

            users.Add(Context.ConnectionId, email);

            await Groups.AddToGroupAsync(Context.ConnectionId, email);

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception e)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, users[Context.ConnectionId]);
            users.Remove(Context.ConnectionId);

            await base.OnDisconnectedAsync(e);
        }
    }
}
