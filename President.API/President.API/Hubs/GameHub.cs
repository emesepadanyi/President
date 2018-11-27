using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using President.API.Controllers;
using President.API.ViewModels;
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

        public override Task OnConnectedAsync()
        {
            var userName = Context.UserIdentifier;
            UserList.TryAdd(Context.ConnectionId, userName);

            var game = GameController.Games.ToList().Find(_game => _game.IsUserInTheGame(userName));
            if(game != null)
            {
                UsersStatusViewModel usersStatusViewModel = getUsersStatus(game.Players());

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

        private UsersStatusViewModel getUsersStatus(List<string> players)
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
            //check if they are in a game
            var game = GameController.Games.ToList().Find(_game => _game.IsUserInTheGame(userName));
            if (game != null)
            {
                //send the other users notice
                //remove the game?
            }
            UserList.TryRemove(Context.ConnectionId, out userName);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
