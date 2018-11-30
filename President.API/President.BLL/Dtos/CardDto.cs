using President.BLL.Game;

namespace President.BLL.Dtos
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
