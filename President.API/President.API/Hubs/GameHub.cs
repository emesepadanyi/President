using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using President.API.Controllers;
using President.API.ViewModels;
using President.BLL.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.Hubs
{

    [Authorize(Policy = "ApiUser")]
    public class GameHub : Hub<IGameHub>
    {
        public static ConcurrentDictionary<string, string> UserList { get; } = new ConcurrentDictionary<string, string>();
        private IGameService gameService;

        public GameHub(IGameService _gameService)
        {
            gameService = _gameService;
        }
        public override Task OnConnectedAsync()
        {
            var userName = Context.UserIdentifier;
            UserList.TryAdd(Context.ConnectionId, userName);

            //this could be better
            var game = GameService.Games.ToList().Find(_game => _game.IsUserInTheGame(userName));

            if(game != null)
            {
                UsersStatusViewModel usersStatusViewModel = GetUsersStatus(game.Players());

                Clients.Users(game.Players()).UserConnected(usersStatusViewModel);

                if(usersStatusViewModel.UsersStatus.All(status => status.Online))
                {
                    System.Threading.Thread.Sleep(1000);

                    var nextUser = game.GetNextUser();
                    foreach (var userId in game.Players())
                    {
                        Clients.User(userId).StartGame(new GameViewModel() { Cards = game.Cards(userId), OwnRank = game.GetRank(userId), Hands = game.HandStatus(userId), NextUser = nextUser, Round = game.Rounds });
                    }
                }
            }

            return base.OnConnectedAsync();
        }

        private UsersStatusViewModel GetUsersStatus(List<string> players)
        {
            List<UserStatusViewModel> usersStatus = new List<UserStatusViewModel>();
            players.ForEach(player =>
            {
                usersStatus.Add(new UserStatusViewModel() { UserName = player, Online = UserList.Values.Contains(player) });
            });

            return new UsersStatusViewModel() { UsersStatus = usersStatus };
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userName = Context.UserIdentifier;

            List<string> userNames = gameService.UserLoggedOff(userName);
            //userNames.ForEach(user => { /*decide what to do*/ });

            UserList.TryRemove(Context.ConnectionId, out userName);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
