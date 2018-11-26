using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace President.API.Hubs
{

    [Authorize(Policy = "ApiUser")]
    public class GameHub : Hub<IGameHub>
    {
        public static ConcurrentDictionary<string, string> UserList { get; } = new ConcurrentDictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            UserList.TryAdd(Context.ConnectionId, Context.UserIdentifier);
            //check if they have pending game
            //send data to all users in the game
            //if everyone is connected
            //start game
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            //check if they are in a game
            //send the other users notice
            //remove the game?
            UserList.TryRemove(Context.ConnectionId, out userId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
