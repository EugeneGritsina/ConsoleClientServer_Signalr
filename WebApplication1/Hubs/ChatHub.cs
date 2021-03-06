﻿using Microsoft.AspNetCore.SignalR;
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

        public async Task AddLetter(char letter)
        {
            await Clients.All.SendAsync("AddLetter", letter);
        }

        public async Task DeleteLetter(string user)
        {
            await Clients.All.SendAsync("DeleteLetter", user);
        }


        //public async Task IsTyping(string user, string message)
        //{
        //    await Clients.All.SendAsync("SayWhoIsTyping", user, message);
        //}
    }
}
