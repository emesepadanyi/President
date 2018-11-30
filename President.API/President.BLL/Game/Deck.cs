using System;
using System.Collections.Generic;
using System.Linq;

namespace President.BLL.Game
{
    public class Deck
    {
        public List<Card> Cards { get; set; } = new List<Card>();

        public Deck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (CardNames name in Enum.GetValues(typeof(CardNames)))
                {
                    Cards.Add(new Card { CardName = name, Suit = suit });
                }
            }

            Shuffle();
        }

        private void Shuffle()
        {
            Random r = new Random();
            for (int n = Cards.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                Card temp = Cards[n];
                Cards[n] = Cards[k];
                Cards[k] = temp;
            }
        }

        public List<Card> DealNCards(int N)
        {
            List<Card> fewCards = new List<Card>(N);
            fewCards = Cards.GetRange(0, N).OrderBy(card => card.CardName).ToList();
            Cards.RemoveRange(0, N);
            return fewCards;
        }
    }
}
