namespace President.BLL.Game
{
    public class Card
    {
        public Suit Suit { get; set; }
        public CardNames CardName { get; set; }
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
