using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using President.API.Dtos;
using President.API.Game;
using President.API.Helpers;
using President.API.Hubs;
using President.API.ViewModels;
using President.DAL.Context;
using President.DAL.Entities;
using System;
using System.Collections.Concurrent;
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

        private static readonly ConcurrentBag<OnlineGame> Games = new ConcurrentBag<OnlineGame>();

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
            var user = await GetUserAsync();

            //check if the enemies are valid & online & not in other game!!!

            OnlineGame game = new OnlineGame(userIDs);

            Games.Add(game);

            string retMessage = string.Empty;
            try
            {
                var nextUser = game.GetNextUser();
                foreach (var userId in userIDs)
                {
                    await gameContext.Clients.User(userId).StartGame(new GameViewModel() { Cards = game.Cards(userId), Hands = game.HandStatus(userId), NextUser = nextUser });
                }
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }
            return retMessage;
        }

        [HttpPost("card")]
        public async Task<IActionResult> ThrowCardAsync([FromBody]CardDto cardDto)
        {
            var user = await GetUserAsync();
            var card = new Card() { CardName = cardDto.name.ToCardNameEnum(), Suit = Enum.Parse<Suit>(cardDto.suit) };

            try
            {
                var game = Games.ToList().Find(_game => _game.IsUserInTheGame(user.UserName));

                game.ThrowCard(user.UserName, card);
                //check if everyone is still online

                var nextUser = game.GetNextUser();
                foreach (var userId in game.Players())
                {
                    await gameContext.Clients.User(userId).PutCard(new MoveViewModel() { Cards = game.Cards(userId), Hands = game.HandStatus(userId), NextUser = nextUser, MovedCard = cardDto });
                }
            }
            catch (Exception)
            {
                return new BadRequestObjectResult(Errors.AddErrorToModelState("validation_faliure", "User cannot throw this card", ModelState));
            }
            return new OkObjectResult("Card thrown");
        }

        private async Task<User> GetUserAsync()
        {
            var userID = _caller.Claims.Single(c => c.Type == "id");
            var user = await _appDbContext.Users.SingleAsync(dbUser => dbUser.Id == userID.Value);
            return user;
        }
    }
}
