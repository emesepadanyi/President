using President.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace President.API.Game
{

    public class OnlineGame
    {
        private List<string> OrderOfPlayers { get; }
        private Dictionary<string, Hand> Hands { get; } = new Dictionary<string, Hand>();
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

        public List<string> Players() => OrderOfPlayers;

        public List<ViewModels.Hand> HandStatus(string playerId)
        {
            var handStatus = new List<ViewModels.Hand>();

            foreach (var hand in Hands)
            {
                handStatus.Add(new ViewModels.Hand() { UserName = hand.Key, NoCards = hand.Value.Cards.Count, Rank = hand.Value.Rank.ToString()});
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
            string user = null;
            if (IsGameStuck()) user = OrderOfPlayers[OrderOfPlayers.Count - 1]; // if the game is stuck than it can be the last person who put the card or passed
            while (user == null || !Hands[user].Active) // if the person put a card, than that person should be up, otherwise the only one who is still active
            {
                user = OrderOfPlayers[0];
                OrderOfPlayers.RemoveAt(0);
                OrderOfPlayers.Add(user);
            }
            return user;
        }

        internal string GetRank(string userId) => Hands[userId].Rank.ToString();

        public bool IsUserInTheGame(string userName) => (Hands[userName] != null);

        public void ThrowCard(string userName, Card card)
        {
            ValidateThrowing(userName, card);
            
            ThrownCards.Insert(0, GetCardFromUser(userName, card));

            if (Hands[userName].Cards.Count == 0) GameLogic.GiveRank(Hands, userName);
        }

        private Card GetCardFromUser(string userName, Card card)
        {
            var theirCard =  Hands[userName].Cards.Find(TheirCard => (TheirCard.Suit == card.Suit && TheirCard.CardName == card.CardName));
            Hands[userName].Cards.Remove(theirCard);
            return theirCard;
        }

        private void ValidateThrowing(string userName, Card card)
        {
            if (OrderOfPlayers[OrderOfPlayers.Count-1] != userName) throw new System.Exception("Not your turn!");
            if (!Hands[userName].Active) throw new System.Exception("You already passed once!");
            if (ThrownCards.Count != 0 && !GameLogic.IsValidMove(ThrownCards[0], card)) throw new System.Exception("You don't own this card!");
        }

        public void Pass(string userName)
        {
            ValidatePassing(userName);
            SetUserInactive(userName);
        }

        private void ValidatePassing(string userName)
        {
            if (OrderOfPlayers[this.OrderOfPlayers.Count - 1] != userName) throw new System.Exception("Not your turn!");
            if (!Hands[userName].Active) throw new System.Exception("You already passed once!");
        }
        private void SetUserInactive(string userName) => Hands[userName].Active = false;

        public bool IsGameStuck() => GameLogic.IsGameStuck(ThrownCards.Count != 0 ? ThrownCards[0] : null, Hands);

        public void ResetThrowingDeck() => ThrownCards.Clear();

        public void ResetActivity() => Hands.ToList().ForEach(action: hand => { if(hand.Value.Cards.Count != 0) hand.Value.Active = true; });
    }
}
