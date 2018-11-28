﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly PresidentDbContext presidentDbContext;
        private readonly IHubContext<GameHub, IGameHub> gameContext;
        private readonly IHubContext<OnlineHub, IOnlineHub> onlineContext;
        private readonly User user;

        public static BlockingCollection<OnlineGame> Games { get; } = new BlockingCollection<OnlineGame>();

        public GameController(
            IHttpContextAccessor httpContextAccessor,
            PresidentDbContext _presidentDbContext,
            IHubContext<GameHub, IGameHub> _gameContext,
            IHubContext<OnlineHub, IOnlineHub> _onlineContext)
        {
            presidentDbContext = _presidentDbContext;
            gameContext = _gameContext;
            onlineContext = _onlineContext;

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
        public async Task<IActionResult> PostAsync([FromBody]string[] userNames)
        {
            try
            {
                CheckUsers(userNames);

                Games.Add(new OnlineGame(userNames));

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
                && !Games.Select(game => game.Players()).Any(players =>  players.Contains(userName)); //user is not in a game
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
                //check if everyone is still online

                var card = new Card() { CardName = cardDto.Name.ToCardNameEnum(), Suit = Enum.Parse<Suit>(cardDto.Suit) };

                var game = Games.ToList().Find(_game => _game.IsUserInTheGame(user.UserName));

                game.ThrowCard(user.UserName, card);

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
                //check if everyone is still online

                var game = Games.ToList().Find(_game => _game.IsUserInTheGame(user.UserName));

                game.Pass(user.UserName);

                await NotifyUsers(null, game);
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
                cardDots.ForEach(cardDto => cards.Add(new Card() { CardName = cardDto.Name.ToUpper().ToCardNameEnum(), Suit = Enum.Parse<Suit>(cardDto.Suit) }));

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
                    foreach (var userId in game.Players())
                    {
                        var stats = SaveStats(game, winners, userId);

                        await gameContext.Clients.User(userId).GameEnded(new EndStatisticsViewModel() { ScoreCard = game.GetScoreCard(), Stats = stats });
                    }

                    presidentDbContext.SaveChanges();

                    Games.TryTake(out game);
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

        private PlayerStatistics SaveStats(OnlineGame game, IEnumerable<string> winners, string userId)
        {
            var stats = presidentDbContext.PlayerStatistics.SingleOrDefault(s => s.User.UserName.Equals(userId));
            if (stats == null)
            {
                var usr = presidentDbContext.Users.Single(dbUser => dbUser.UserName == userId);
                stats = new PlayerStatistics() { User = usr, GamesPlayed = 0, SumPointsEarned = 0, TimesWon = 0 };
                presidentDbContext.Add(stats);
            }
            stats.GamesPlayed += 1;
            stats.SumPointsEarned += game.GetTotalPoints(userId);
            if (winners.Contains(userId)) stats.TimesWon += 1;

            return stats;
        }
    }
}
