using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.Hubs
{
    public class ChatHub : Hub<ITypedHubClient>
    {
    }
}
