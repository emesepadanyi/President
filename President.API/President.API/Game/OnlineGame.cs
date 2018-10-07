using President.API.Dtos;
using System.Collections.Generic;

namespace President.API.Game
{

    public class OnlineGame
    {
       // private List<User> Players { get; }
        private Dictionary<string, Game.Hand> Hands { get; } = new Dictionary<string, Game.Hand>();


        public OnlineGame(string[] playerIds)
        {
            Deck deck = new Deck();
            foreach (var player in playerIds)
            {
                Hands.Add(player, new Game.Hand(deck.dealNCards(13)));
            }
        }

        public List<CardDto> Cards(string playerId)
        {
            List<CardDto> cards = new List<CardDto>();
            foreach (var card in Hands[playerId].Cards)
            {
                cards.Add(new CardDto(card));
            }
            return cards;
        }

        public List<ViewModels.Hand> HandStatus(string playerId)
        {
            var handStatus = new List<ViewModels.Hand>();

            foreach (var hand in Hands)
            {
                handStatus.Add(new ViewModels.Hand() { UserName = hand.Key, NoCards = hand.Value.Cards.Count});
            }

            while(handStatus[0].UserName != playerId)
            {
                var temp = handStatus[0];
                handStatus.RemoveAt(0);
                handStatus.Add(temp);
            }
            handStatus.RemoveAt(0);

            return handStatus;
        }
    }
}
