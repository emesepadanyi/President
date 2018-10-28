using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.Game
{
    public class Hand
    {
        public List<Card> Cards { get; set; }
        public bool Active { get; set; }
        public Rank Rank { get; set; }
        public Hand(List<Card> cards)
        {
            Cards = cards;
            Active = true;
            Rank = Rank.AverageJoe;
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
