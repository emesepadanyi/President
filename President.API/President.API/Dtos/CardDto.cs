using President.API.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.Dtos
{
    public class CardDto
    {
        public string suit { get; set; }
        public string name { get; set; }

        public CardDto(Card card)
        {
            suit = card.Suit.ToString();
            name = card.CardName.ToCardString();
        }

        public CardDto() { }
    }
}
