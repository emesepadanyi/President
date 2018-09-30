using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.Game
{
    public class Deck
    {
        public List<Card> cards { get; set; } = new List<Card>();

        public Deck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (CardNames name in Enum.GetValues(typeof(CardNames)))
                {
                    cards.Add(new Card { cardName = name, suit = suit });
                }
            }

            Shuffle();
        }

        private void Shuffle()
        {
            Random r = new Random();
            for (int n = cards.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                Card temp = cards[n];
                cards[n] = cards[k];
                cards[k] = temp;
            }
        }

        public List<Card> dealNCards(int N)
        {
            List<Card> fewCards = new List<Card>(N);
            fewCards = cards.GetRange(0, N);
            cards.RemoveRange(0, N);
            return fewCards;
        }
    }
}
