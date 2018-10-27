using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using President.API.Dtos;
using President.API.Game;
using President.API.Helpers;
using President.API.Hubs;
using President.API.ViewModels;
using President.DAL.Context;
using President.DAL.Entities;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace President.API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly User user;
        private readonly PresidentDbContext presidentDbContext;
        private IHubContext<GameHub, IGameHub> gameContext;

        private static readonly ConcurrentBag<OnlineGame> Games = new ConcurrentBag<OnlineGame>();

        public GameController(
            IHttpContextAccessor httpContextAccessor,
            PresidentDbContext _presidentDbContext,
            IHubContext<GameHub, IGameHub> _gameContext)
        {
            presidentDbContext = _presidentDbContext;
            gameContext = _gameContext;

            ClaimsPrincipal caller = httpContextAccessor.HttpContext.User;
            user = GetUser(caller);
        }
        private User GetUser(ClaimsPrincipal caller)
        {
            var userID = caller.Claims.Single(c => c.Type == "id");
            var user = presidentDbContext.Users.Single(dbUser => dbUser.Id == userID.Value);
            return user;
        }

        [HttpPost]
        public async Task<string> PostAsync([FromBody]string[] userIDs)
        {
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
            try
            {
                var card = new Card() { CardName = cardDto.name.ToCardNameEnum(), Suit = Enum.Parse<Suit>(cardDto.suit) };

                var game = Games.ToList().Find(_game => _game.IsUserInTheGame(user.UserName));

                game.ThrowCard(user.UserName, card);
                //check if everyone is still online

                await NotifyUsers(cardDto, user, game);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult(Errors.AddErrorToModelState("validation_faliure", "User cannot throw this card", ModelState));
            }
            return new OkObjectResult("Card thrown");
        }

        [HttpPost("pass")]
        public async Task<IActionResult> PassAsync()
        {
            try
            {
                var game = Games.ToList().Find(_game => _game.IsUserInTheGame(user.UserName));

                game.Pass(user.UserName);

                await NotifyUsers(null, user, game);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(Errors.AddErrorToModelState("validation_faliure", e.Message , ModelState));
            }
            return new OkObjectResult("User passed");
        }

        private async Task NotifyUsers(CardDto cardDto, User user, OnlineGame game)
        {
            var nextUser = game.GetNextUser();

            foreach (var userId in game.Players())
            {
                await gameContext.Clients.User(userId).PutCard(new MoveViewModel() { Cards = game.Cards(userId), Hands = game.HandStatus(userId), NextUser = nextUser, MovedCard = cardDto });
            }

            if (game.IsGameStuck())
            {
                System.Threading.Thread.Sleep(1000);
                game.ResetThrowingDeck();
                foreach (var userId in game.Players())
                {
                    await gameContext.Clients.User(userId).ResetDeck(nextUser);
                }
                game.ResetActivity();
            }
        }
    }
}
