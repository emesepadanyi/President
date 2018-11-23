using President.API.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.Dtos
{
    public class CardDto
    {
        public string Suit { get; set; }
        public string Name { get; set; }

        public CardDto(Card card)
        {
            Suit = card.Suit.ToString();
            Name = card.CardName.ToCardString();
        }

        public CardDto() { }
    }
}
