using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace President.API.Hubs
{

    [Authorize(Policy = "ApiUser")]
    public class GameHub : Hub<IGameHub>
    {
    }
}
