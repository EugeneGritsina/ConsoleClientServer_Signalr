using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace WebApplication1.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task Typing(string user, string message)
        {
            await Clients.Others.SendAsync("Typing", user, message);
        }
    }
}
