using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using President.API.Hubs;
using President.API.ViewModels;
using President.DAL.Context;
using President.DAL.Entities;
using System;
using System.Linq;
using System.Security.Claims;

namespace President.API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private readonly ClaimsPrincipal _caller;
        private readonly PresidentDbContext _appDbContext;
        private IHubContext<ChatHub, ITypedHubClient> _hubContext;

        public MessageController(
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            PresidentDbContext appDbContext,
            IHubContext<ChatHub, ITypedHubClient> hubContext)
        {
            _caller = httpContextAccessor.HttpContext.User;
            _appDbContext = appDbContext;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<string> PostAsync([FromBody]string msg)
        {
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var user = await _appDbContext.Users.SingleAsync(dbUser => dbUser.Id == userId.Value);

            string retMessage = string.Empty;
            try
            {
                await _hubContext.Clients.All.BroadcastMessage(new MessageViewModel() { UserName = user.UserName, Message = msg });
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
