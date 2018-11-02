using System.Collections.Generic;

namespace President.API.Game
{
    public class Hand
    {
        public List<Card> Cards { get; set; }
        public bool Active { get; set; }
        public Rank? Rank { get; set; }
        public Hand(List<Card> cards)
        {
            Cards = cards;
            Active = true;
            Rank = null;
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
