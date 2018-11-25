using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace President.API.Hubs
{
    [Authorize(Policy = "ApiUser")]
    public class OnlineHub : Hub
    {
        public static ConcurrentDictionary<string, string> UserList = new ConcurrentDictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            UserList.TryAdd(Context.ConnectionId, Context.UserIdentifier);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            UserList.TryRemove(Context.ConnectionId, out userId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
