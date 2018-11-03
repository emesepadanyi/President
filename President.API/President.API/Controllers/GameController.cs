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
        public async Task<IActionResult> PostAsync([FromBody]string[] userIDs)
        {
            try
            {
                //check if the enemies are valid & online & not in other game

                OnlineGame game = new OnlineGame(userIDs);
                Games.Add(game);

                await DealCards(game);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(Errors.AddErrorToModelState("validation_faliure", e.Message, ModelState));
            }
            return new OkObjectResult("Game started");
        }

        [HttpPost("card")]
        public async Task<IActionResult> ThrowCardAsync([FromBody]CardDto cardDto)
        {
            try
            {

                //check if everyone is still online

                var card = new Card() { CardName = cardDto.name.ToCardNameEnum(), Suit = Enum.Parse<Suit>(cardDto.suit) };

                var game = Games.ToList().Find(_game => _game.IsUserInTheGame(user.UserName));

                game.ThrowCard(user.UserName, card);

                await NotifyUsers(cardDto, user, game);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(Errors.AddErrorToModelState("validation_faliure", e.Message, ModelState));
            }
            return new OkObjectResult("Card thrown");
        }

        [HttpPost("pass")]
        public async Task<IActionResult> PassAsync()
        {
            try
            {
                //check if everyone is still online

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

        [HttpPost("switch")]
        public async Task<IActionResult> SwitchAsync([FromBody]List<CardDto> cardDots)
        {
            try
            {
                //check if everyone is still online

                var game = Games.ToList().Find(_game => _game.IsUserInTheGame(user.UserName));

                var cards = new List<Card>();
                cardDots.ForEach(cardDto => cards.Add(new Card() { CardName = cardDto.name.ToCardNameEnum(), Suit = Enum.Parse<Suit>(cardDto.suit) }));

                game.Switch(user.UserName, cards);

                if (game.IsSwitchingOver())
                {
                    game.StartNextRound();
                    await DealCards(game);
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(Errors.AddErrorToModelState("validation_faliure", e.Message, ModelState));
            }
            return new OkObjectResult("User passed");
        }

        private async Task DealCards(OnlineGame game)
        {
            var nextUser = game.GetNextUser();
            foreach (var userId in game.Players())
            {
                await gameContext.Clients.User(userId).StartGame(new GameViewModel() { Cards = game.Cards(userId), OwnRank = game.GetRank(userId), Hands = game.HandStatus(userId), NextUser = nextUser, Round = game.Rounds });
            }
        }

        private async Task NotifyUsers(CardDto cardDto, User user, OnlineGame game)
        {
            var nextUser = game.GetNextUser();

            foreach (var userId in game.Players())
            {
                await gameContext.Clients.User(userId).PutCard(new MoveViewModel() { Cards = game.Cards(userId), OwnRank = game.GetRank(userId), Hands = game.HandStatus(userId), NextUser = nextUser, MovedCard = cardDto });
            }

            if (game.IsRoundOver())
            {
                System.Threading.Thread.Sleep(1000);
                game.PrepareNextRound();

                foreach (var userId in game.Players())
                {
                    NewRoundViewModel nrvm = null;
                    if (game.IsLeader(userId)) {
                        nrvm = new NewRoundViewModel() { Wait = false, SwitchedCards = game.GetSwitchableCards(userId), Cards = game.Cards(userId), OwnRank = game.GetRank(userId) };
                    } else {
                        nrvm = new NewRoundViewModel() { Wait =  true, SwitchedCards = game.GetSwitchableCards(userId), Cards = game.Cards(userId), OwnRank = game.GetRank(userId) };
                    }
                    await gameContext.Clients.User(userId).WaitForNewRound(nrvm);
                }
            }
            else if (game.IsGameStuck())
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
