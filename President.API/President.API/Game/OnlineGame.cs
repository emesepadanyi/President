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
        public int Rounds { get; set; } = 1;

        private IGameLogic GameLogic { get; }

        public OnlineGame(string[] playerIds)
        {
            GameLogic = new DefaultGameLogic();

            foreach (var player in playerIds)
            {
                Hands.Add(player, new Hand(null));
            }

            DealCardsForPlayers();

            OrderOfPlayers = new List<string>(playerIds);
        }

        private void DealCardsForPlayers()
        {
            Deck deck = new Deck();
            foreach (var player in Hands)
            {
                player.Value.Cards = deck.DealNCards(13);
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

            if (IsRoundOver())
            {
                user = Hands.Where(predicate: (hand) => hand.Value.Cards.Count != 0).First().Key;
                Hands[user].Rank = Rank.Scum;
                Hands[user].Score.Add(1);
                Hands[user].Active = true;
                while(user != OrderOfPlayers[OrderOfPlayers.Count-1])
                {
                    var tempUser = OrderOfPlayers[0];
                    OrderOfPlayers.RemoveAt(0);
                    OrderOfPlayers.Add(tempUser);
                }
            }
            else if (IsGameStuck())
            {
                user = OrderOfPlayers[OrderOfPlayers.Count - 1];
            }

            while (user == null || !Hands[user].Active)
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
            if (ThrownCards.Count != 0 && !GameLogic.IsValidMove(ThrownCards[0], card)) throw new System.Exception("You have to top the previous card!");
        }

        public void Switch(string userName, List<Card> cards)
        {
            ValidateSwitching(userName, cards);
            SwitchCards(userName, cards);
        }

        public void StartNextRound()
        {
            Hands.ToList().ForEach(action: hand =>
            {
                hand.Value.SwitchedCards = null;
                hand.Value.Rank = null;
            });
        }

        private void SwitchCards(string userName, List<Card> cards)
        {
            cards.ForEach(card => GetCardFromUser(userName, card));
            Hands[userName].Cards.AddRange(Hands[userName].SwitchedCards);
            Hands[userName].Cards = Hands[userName].Cards.OrderBy(card => card.CardName).ToList();

            switch (Hands[userName].Rank)
            {
                case (Rank.President):
                    var enemyCards = Hands.ToList()
                        .Find(hand => hand.Value.Rank == Rank.Scum).Value.Cards;
                    enemyCards.AddRange(cards);
                    enemyCards.OrderBy(card => card.CardName).ToList();
                    break;
                case (Rank.VicePresident):
                    enemyCards = Hands.ToList()
                        .Find(hand => hand.Value.Rank == Rank.ViceScum).Value.Cards;
                    enemyCards.AddRange(cards);
                    enemyCards.OrderBy(card => card.CardName).ToList();
                    break;
            }
        }

        private void ValidateSwitching(string userName, List<Card> cards)
        {
            if (!(Hands[userName].Rank == Rank.President || Hands[userName].Rank == Rank.VicePresident)) throw new Exception("You are not int he position to switch cards!");
            if (!((Hands[userName].Rank == Rank.President && cards.Count == 2)
                || (Hands[userName].Rank == Rank.VicePresident && cards.Count == 1))) throw new Exception("Didn't chose the correct number of cards!");
        }

        public bool IsSwitchingOver()
        {
            int noCards = Hands.First().Value.Cards.Count;
            return Hands.All(hand => hand.Value.Cards.Count == noCards);
        }

        public List<ScoreDto> GetScoreCard()
        {
            List<ScoreDto> scores = new List<ScoreDto>();
            this.Hands.ToList().ForEach(hand =>
            {
                scores.Add(new ScoreDto() { UserName = hand.Key, Points = hand.Value.Score.Points, Total = hand.Value.Score.Total });
            });
            return scores;
        }

        public int GetTotalPoints(string userName) => Hands[userName].Score.Total;

        public IEnumerable<string> Winners()
        {
            int max = Hands.Select(hand => hand.Value.Score.Total).Max();
            var winners = Hands.Where(hand => hand.Value.Score.Total == max).Select(winner => winner.Key);
            return winners;
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

        public void PrepareNextRound()
        {
            Rounds++;
            ResetThrowingDeck();
            DealCardsForPlayers();
            Hands.ToList().ForEach(action: hand => hand.Value.Active = true);
        }

        public void ResetThrowingDeck() => ThrownCards.Clear();
        public void ResetActivity() => Hands.ToList().ForEach(action: hand => { if(hand.Value.Cards.Count != 0) hand.Value.Active = true; });

        public bool IsGameOver() => (Rounds == 10);
        public bool IsRoundOver() => (Hands.Where(predicate: (hand) => hand.Value.Cards.Count != 0).Count() == 1);
        public bool IsLeader(string userName) => (Hands[userName].Rank == Rank.VicePresident || Hands[userName].Rank == Rank.President);
        public List<CardDto> GetSwitchableCards(string userName)
        {
            List<CardDto> changedCards = new List<CardDto>();
            
            switch (Hands[userName].Rank)
            {
                case (Rank.President):
                    Hands[userName].SwitchedCards = Hands.ToList()
                        .Find(hand => hand.Value.Rank == Rank.Scum).Value
                        .SwitchedCards;
                    break;
                case (Rank.VicePresident):
                    Hands[userName].SwitchedCards = Hands.ToList()
                        .Find(hand => hand.Value.Rank == Rank.ViceScum).Value
                        .SwitchedCards;
                    break;
            }

            Hands[userName].SwitchedCards.ForEach(card => changedCards.Add(new CardDto(card)));
            return changedCards;
        }
    }
}
