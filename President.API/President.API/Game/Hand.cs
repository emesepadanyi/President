using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.Game
{
    public class Hand
    {
        public List<Card> Cards { get; }
        public Hand(List<Card> cards)
        {
            Cards = cards;
        }
    }
}
