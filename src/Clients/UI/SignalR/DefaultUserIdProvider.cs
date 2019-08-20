using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.SignalR
{
    public class DefaultUserIdProvider : IUserIdProvider
    {
        public const string UserId = "11111111-1111-1111-1111-111111111111";

        public string GetUserId(HubConnectionContext connection)
        {
            return UserId;
        }
    }
}
