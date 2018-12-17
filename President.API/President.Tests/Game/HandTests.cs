using President.BLL.Game;
using System.Collections.Generic;
using Xunit;

namespace President.Tests.Game
{
    public class HandTests
    {

        [Fact]
        public void CreateHand_WithTwoCards()
        {
            var hand = new Hand(new List<Card> { new Card() { Suit = Suit.clubs, CardName = CardNames.ace },
                                                 new Card() { Suit = Suit.diams, CardName = CardNames.ace }});
            Assert.True(hand.Active);
            Assert.Null(hand.Rank);
            Assert.NotNull(hand.Score);
            Assert.Equal(2, hand.Cards.Count);
        }

        [Fact]
        public void GetSwitchedCards_FromScum()
        {
            var hand = new Hand(new List<Card> { new Card() { Suit = Suit.clubs, CardName = CardNames.ace },
                                                 new Card() { Suit = Suit.diams, CardName = CardNames.ace }});

            hand.Rank = Rank.Scum;
            var switchedCards = hand.SwitchedCards;

            Assert.Equal(2, switchedCards.Count);
        }

        [Fact]
        public void GetSwitchedCards_FromViceScum()
        {
            var hand = new Hand(new List<Card> { new Card() { Suit = Suit.clubs, CardName = CardNames.ace },
                                                 new Card() { Suit = Suit.diams, CardName = CardNames.ace }});

            hand.Rank = Rank.ViceScum;
            var switchedCards = hand.SwitchedCards;

            Assert.Single(switchedCards);
        }

        [Fact]
        public void GetSwitchedCards_FromAverageJoe()
        {
            var hand = new Hand(new List<Card> { new Card() { Suit = Suit.clubs, CardName = CardNames.ace },
                                                 new Card() { Suit = Suit.diams, CardName = CardNames.ace }});

            hand.Rank = Rank.AverageJoe;
            var switchedCards = hand.SwitchedCards;

            Assert.Empty(switchedCards);
        }

        [Fact]
        public void SetSwitchedCards_ReturnCards()
        {
            var hand = new Hand(new List<Card>());

            hand.Rank = Rank.President;
            hand.SwitchedCards = new List<Card> { new Card() { Suit = Suit.clubs, CardName = CardNames.ace },
                                                 new Card() { Suit = Suit.diams, CardName = CardNames.ace }};

            Assert.Equal(2, hand.SwitchedCards.Count);
        }
    }
}
