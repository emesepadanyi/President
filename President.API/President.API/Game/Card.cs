using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.Game
{
    public class Card
    {
        public Suit suit { get; set; }
        public CardNames cardName { get; set; }
    }

    public enum Suit
    {
        clubs, diams, hearts, spades
    }
    public enum CardNames
    {
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
        ten,
        jack,
        queen,
        king,
        ace
    }
}
