using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using President.BLL.Dtos;
using President.BLL.Game;
using President.BLL.Services;
using President.API.Helpers;
using President.API.Hubs;
using President.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly IGameService gameService;
        private readonly IHubContext<GameHub, IGameHub> gameContext;
        private readonly IHubContext<OnlineHub, IOnlineHub> onlineContext;
        private readonly string userName;

        public GameController(
            IHttpContextAccessor httpContextAccessor,
            IGameService _gameService,
            IHubContext<GameHub, IGameHub> _gameContext,
            IHubContext<OnlineHub, IOnlineHub> _onlineContext)
        {
            gameService = _gameService;
            gameContext = _gameContext;
            onlineContext = _onlineContext;

            userName = gameService.GetUserName(httpContextAccessor.HttpContext.User);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]string[] userNames)
        {
            try
            {
                CheckUsers(userNames);

                gameService.AddGame(new OnlineGame(userNames));

                await InviteUsersAsync(userNames);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(Errors.AddErrorToModelState("validation_faliure", e.Message, ModelState));
            }
            return new OkObjectResult("Game started");
        }

        private void CheckUsers(string[] userNames)
        {
            var valid = userNames.All(userName =>
            {
                return OnlineHub.UserList.Values.Contains(userName) //user is online
                && !gameService.UserIsInAnyGame(userName);
            });

            if(!valid)
            {
                throw new Exception("Not all users are available to play!");
            }
        }

        private async Task InviteUsersAsync(string[] userNames)
        {
            userNames = userNames.Reverse().Take(3).ToArray();
            await onlineContext.Clients.Users(userNames).Invite();
        }

        [HttpPost("card")]
        public async Task<IActionResult> ThrowCardAsync([FromBody]CardDto cardDto)
        {
            try
            {
                var game = gameService.ThrowCard(cardDto, userName);

                await NotifyUsers(cardDto, game);
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
                var game = gameService.Pass(userName);

                await NotifyUsers(null, game);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(Errors.AddErrorToModelState("validation_faliure", e.Message , ModelState));
            }
            return new OkObjectResult("User passed");
        }

        [HttpPost("switch")]
        public async Task<IActionResult> SwitchAsync([FromBody]List<CardDto> cardDtos)
        {
            try
            {
                var game = gameService.Switch(userName, cardDtos);

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

        private async Task NotifyUsers(CardDto cardDto, OnlineGame game)
        {
            var nextUser = game.GetNextUser();

            foreach (var userId in game.Players())
            {
                await gameContext.Clients.User(userId).PutCard(new MoveViewModel() { Cards = game.Cards(userId), OwnRank = game.GetRank(userId), Hands = game.HandStatus(userId), NextUser = nextUser, MovedCard = cardDto });
            }

            if (game.IsRoundOver())
            {
                if (game.IsGameOver())
                {
                    System.Threading.Thread.Sleep(1000);

                    var winners = game.Winners();
                    var stats = gameService.SaveStats(game, winners, game.Players());

                    foreach (var userName in game.Players())
                    {
                        await gameContext.Clients.User(userName).GameEnded(new EndStatisticsViewModel() { ScoreCard = game.GetScoreCard(), Stats = stats.First(stat => stat.User.UserName == userName) });
                    }
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                    game.PrepareNextRound();

                    foreach (var userId in game.Players())
                    {
                        await gameContext.Clients.User(userId).WaitForNewRound(new NewRoundViewModel()
                        {
                            Wait = !game.IsLeader(userId),
                            SwitchedCards = game.GetSwitchableCards(userId),
                            Cards = game.Cards(userId),
                            OwnRank = game.GetRank(userId),
                            ScoreCard = game.GetScoreCard()
                        });
                    }
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
