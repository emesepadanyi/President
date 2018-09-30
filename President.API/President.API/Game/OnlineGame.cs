using President.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.Game
{

    public class OnlineGame
    {
       // private List<User> Players { get; }
        private Dictionary<string, Hand> Hands { get; } = new Dictionary<string, Hand>();


        public OnlineGame(string[] playerIds)
        {
            Deck deck = new Deck();
            foreach (var player in playerIds)
            {
                Hands.Add(player, new Hand(deck.dealNCards(13)));
            }
        }

        public List<Card> Cards(string playerId)
        {
            return Hands[playerId].Cards;
        }

        public Dictionary<string, int> HandStatus(string playerId)
        {
            Dictionary<string, int> handStatus = new Dictionary<string, int>();

            foreach (var hand in Hands)
            {
                handStatus.Add(hand.Key, hand.Value.Cards.Count);
            }

            return handStatus;
        }
    }
}
