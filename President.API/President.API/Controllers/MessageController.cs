using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using President.API.Hubs;
using President.API.ViewModels;
using System;

namespace President.API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private IHubContext<ChatHub, ITypedHubClient> _hubContext;

        public MessageController(IHubContext<ChatHub, ITypedHubClient> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public string Post([FromBody]string msg)
        {
            string retMessage = string.Empty;
            try
            {
                _hubContext.Clients.All.BroadcastMessage( msg );
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }
            return retMessage;
        }
    }
}
