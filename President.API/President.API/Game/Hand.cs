using System.Collections.Generic;
using System.Linq;

namespace President.API.Game
{
    public class Hand
    {
        public List<Card> Cards { get; set; }
        public List<Card> SwitchedCards { get; set; }
        public bool Active { get; set; }
        public Rank? Rank { get; set; }
        public Hand(List<Card> cards)
        {
            Cards = cards;
            Active = true;
            Rank = null;
        }

        public List<Card> GetSwitchedCards()
        {
            if(SwitchedCards != null)
            {
                return SwitchedCards;
            }
            else if (Rank == Game.Rank.Scum)
            {
                SwitchedCards = Cards.TakeLast(2).ToList();
                Cards.RemoveRange(Cards.Count-2, 2);
            }
            else if(Rank == Game.Rank.ViceScum)
            {
                SwitchedCards = Cards.TakeLast(1).ToList();
                Cards.RemoveAt(Cards.Count - 1);
            }
            else
            {
                SwitchedCards = new List<Card>();
            }
            return SwitchedCards;
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
