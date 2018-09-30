using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using President.API.Game;
using President.API.Hubs;
using President.API.ViewModels;
using President.DAL.Context;
using President.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace President.API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly ClaimsPrincipal _caller;
        private readonly PresidentDbContext _appDbContext;
        private IHubContext<GameHub, IGameHub> gameContext;

        private List<OnlineGame> Games { get; } = new List<OnlineGame>();

        public GameController(
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            PresidentDbContext appDbContext,
            IHubContext<GameHub, IGameHub> hubContext)
        {
            _caller = httpContextAccessor.HttpContext.User;
            _appDbContext = appDbContext;
            gameContext = hubContext;
        }

        [HttpPost]
        public async Task<string> PostAsync([FromBody]string[] userIDs)
        {
            var userID = _caller.Claims.Single(c => c.Type == "id");
            var user = await _appDbContext.Users.SingleAsync(dbUser => dbUser.Id == userID.Value);

            //check if the enemies are valid & online!!!

            OnlineGame game = new OnlineGame(userIDs);

            Games.Add(game);

            string retMessage = string.Empty;
            try
            {
                foreach (var userId in userIDs)
                {
                    await gameContext.Clients.User(userId).StartGame(new GameViewModel() { Cards = game.Cards(userId), Hands = game.HandStatus(userId) });
                }
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
