using System.Collections.Generic;
using System.Linq;

namespace President.API.Game
{
    public class Hand
    {
        public List<Card> Cards { get; set; }
        public bool Active { get; set; }
        public Rank? Rank { get; set; }
        public Score Score { get; } = new Score();
        public Hand(List<Card> cards)
        {
            Cards = cards;
            Active = true;
            Rank = null;
        }

        private List<Card> switchedCards;
        public List<Card> SwitchedCards {
            get {
                if (switchedCards != null)
                {
                    return switchedCards;
                }
                else if (Rank == Game.Rank.Scum)
                {
                    switchedCards = Cards.TakeLast(2).ToList();
                    Cards.RemoveRange(Cards.Count - 2, 2);
                }
                else if (Rank == Game.Rank.ViceScum)
                {
                    switchedCards = Cards.TakeLast(1).ToList();
                    Cards.RemoveAt(Cards.Count - 1);
                }
                else
                {
                    switchedCards = new List<Card>();
                }
                return switchedCards;
            }
            set {
                switchedCards = value;
            }
        }
    }

    public enum Rank
    {
        Scum,
        ViceScum,
        AverageJoe,
        VicePresident,
        President
    }
}
