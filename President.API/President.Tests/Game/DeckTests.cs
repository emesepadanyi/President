using President.BLL.Game;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace President.Tests.Game
{
    public class DeckTests
    {
        [Fact]
        public void DeckInit_Creates52Cards()
        {
            Deck deck = new Deck();

            Assert.Equal(52, deck.Cards.Count);
        }

        [Fact]
        public void DeckInit_Creates52Cards_13OfEach()
        {
            Deck deck = new Deck();

            Assert.Equal(13, deck.Cards.Where(card => card.Suit == Suit.clubs).Count());
            Assert.Equal(13, deck.Cards.Where(card => card.Suit == Suit.diams).Count());
            Assert.Equal(13, deck.Cards.Where(card => card.Suit == Suit.hearts).Count());
            Assert.Equal(13, deck.Cards.Where(card => card.Suit == Suit.spades).Count());
        }

        [Fact]
        public void DeckInit_Creates52Cards_Shuffled()
        {
            Deck deck = new Deck();
            //random not mocked => might not be always true!!! // TODO !
            //Assert.NotEqual(CardNames.ace, deck.Cards[0].CardName);
        }

        [Fact]
        public void DeckDeal_Returns13Cards_RemainsTheRest()
        {
            Deck deck = new Deck();

            List<Card> hand = deck.DealNCards(13);

            Assert.Equal(13, hand.Count);
            Assert.Equal(52 - 13, deck.Cards.Count);
        }
    }
}
