using Microsoft.EntityFrameworkCore;
using President.BLL.Dtos;
using President.BLL.Game;
using President.DAL.Context;
using President.DAL.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace President.BLL.Services
{
    public class GameService : IGameService
    {
        private PresidentDbContext context;
        public static BlockingCollection<OnlineGame> Games { get; } = new BlockingCollection<OnlineGame>();

        public GameService(PresidentDbContext _context)
        {
            context = _context;
        }

        public virtual string GetUserName(ClaimsPrincipal caller)
        {
            var userID = caller.Claims.Single(c => c.Type == "id");
            var user = context.Users.Single(dbUser => dbUser.Id == userID.Value);
            return user.UserName;
        }

        public virtual void AddGame(OnlineGame game)
        {
            Games.Add(game);
        }

        public virtual bool UserIsInAnyGame(string userName)
        {
            return Games.Select(game => game.Players()).Any(players => players.Contains(userName));
        }

        public virtual OnlineGame ThrowCard(CardDto cardDto, string userName)
        {
            var card = new Card() { CardName = cardDto.Name.ToCardNameEnum(), Suit = Enum.Parse<Suit>(cardDto.Suit) };

            var game = Games.ToList().Find(_game => _game.IsUserInTheGame(userName));

            game.ThrowCard(userName, card);

            return game;
        }

        public virtual OnlineGame Pass(string userName)
        {
            var game = Games.ToList().Find(_game => _game.IsUserInTheGame(userName));

            game.Pass(userName);

            return game;
        }

        public virtual OnlineGame Switch(string userName, List<CardDto> cardDtos)
        {
            var game = Games.ToList().Find(_game => _game.IsUserInTheGame(userName));

            var cards = new List<Card>();
            cardDtos.ForEach(cardDto => cards.Add(new Card() { CardName = cardDto.Name.ToUpper().ToCardNameEnum(), Suit = Enum.Parse<Suit>(cardDto.Suit) }));

            game.Switch(userName, cards);

            return game;
        }

        public virtual IEnumerable<PlayerStatistics> SaveStats(OnlineGame game, IEnumerable<string> winners, IEnumerable<string> userNames)
        {
            var statistics = userNames.Select(userName => {
                var stats = context.PlayerStatistics.Include(s => s.User).SingleOrDefault(s => s.User.UserName.Equals(userName));
                if (stats == null)
                {
                    var usr = context.Users.Single(dbUser => dbUser.UserName == userName);
                    stats = new PlayerStatistics() { User = usr, GamesPlayed = 0, SumPointsEarned = 0, TimesWon = 0 };
                    context.Add(stats);
                }
                stats.GamesPlayed += 1;
                stats.SumPointsEarned += game.GetTotalPoints(userName);
                if (winners.Contains(userName)) stats.TimesWon += 1;

                return stats;
            });

            context.SaveChanges();
            Games.TryTake(out game);

            return statistics;
        }

        public virtual List<string> UserLoggedOff(string userName)
        {
            var game = Games.ToList().Find(_game => _game.IsUserInTheGame(userName));
            if (game != null)
            {
                return game.Players();
            }
            return new List<string>();
        }
    }
}
