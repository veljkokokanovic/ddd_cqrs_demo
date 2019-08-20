using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.SignalR
{
    public class NotificationHub : Hub
    {
        //public Task SendCommandMessage(Guid userId, CommandMessage message)
        //{
        //    return Clients.User(userId.ToString()).SendAsync("CommandMessage", message);
        //}
    }
}
