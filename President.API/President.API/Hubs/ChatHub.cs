using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace President.API.Hubs
{

    [Authorize(Policy = "ApiUser")]
    public class ChatHub : Hub<ITypedHubClient>
    {
    }
}
