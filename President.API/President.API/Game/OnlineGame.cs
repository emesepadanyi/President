using President.API.Dtos;
using System;
using System.Collections.Generic;

namespace President.API.Game
{

    public class OnlineGame
    {
        private List<string> OrderOfPlayers { get; }
        private Dictionary<string, Game.Hand> Hands { get; } = new Dictionary<string, Game.Hand>();
        private List<Card> ThrownCards { get; } = new List<Card>();

        private IGameLogic GameLogic { get; }

        public OnlineGame(string[] playerIds)
        {
            GameLogic = new DefaultGameLogic();

            Deck deck = new Deck();
            foreach (var player in playerIds)
            {
                Hands.Add(player, new Game.Hand(deck.DealNCards(13)));
            }

            OrderOfPlayers = new List<string>(playerIds);
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

        public List<string> Players()
        {
            return this.OrderOfPlayers;
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

        public string GetNextUser()
        {
            var user = this.OrderOfPlayers[0];
            this.OrderOfPlayers.RemoveAt(0);
            this.OrderOfPlayers.Add(user);
            return user;
        }

        public bool IsUserInTheGame(string userName)
        {
            if (this.Hands[userName] != null)
            {
                return true;
            }
            return false;
        }

        public void ThrowCard(string userName, Card card)
        {
            ValidateThrowing(userName, card);
            
            this.ThrownCards.Insert(0, GetCardFromUser(userName, card));
        }

        private Card GetCardFromUser(string userName, Card card)
        {
            var theirCard =  Hands[userName].Cards.Find(TheirCard => (TheirCard.Suit == card.Suit && TheirCard.CardName == card.CardName));
            Hands[userName].Cards.Remove(theirCard);
            return theirCard;
        }

        private void ValidateThrowing(string userName, Card card)
        {
            if (this.OrderOfPlayers[this.OrderOfPlayers.Count-1] != userName) throw new System.Exception("User is not available to throw a card.");
            if (this.ThrownCards.Count != 0 && !this.GameLogic.IsValidMove(this.ThrownCards[0], card)) throw new System.Exception("User is not available to throw this card.");
        }
    }
}
