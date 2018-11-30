using System;

namespace President.BLL.Game
{
    public static class CardExtensions
    {
        public static string ToCardString(this CardNames name)
        {
            switch (name)
            {
                case CardNames.two:
                    return "2";
                case CardNames.three:
                    return "3";
                case CardNames.four:
                    return "4";
                case CardNames.five:
                    return "5";
                case CardNames.six:
                    return "6";
                case CardNames.seven:
                    return "7";
                case CardNames.eight:
                    return "8";
                case CardNames.nine:
                    return "9";
                case CardNames.ten:
                    return "10";
                case CardNames.jack:
                    return "J";
                case CardNames.queen:
                    return "Q";
                case CardNames.king:
                    return "K";
                case CardNames.ace:
                    return "A";
                default: throw new Exception("Not a valid card");
            }
        }

        public static CardNames ToCardNameEnum(this string name)
        {
            switch (name)
            {
                case "2":
                    return CardNames.two;
                case "3":
                    return CardNames.three;
                case "4":
                    return CardNames.four;
                case "5":
                    return CardNames.five;
                case "6":
                    return CardNames.six;
                case "7":
                    return CardNames.seven;
                case "8":
                    return CardNames.eight;
                case "9":
                    return CardNames.nine;
                case "10":
                    return CardNames.ten;
                case "J":
                    return CardNames.jack;
                case "Q":
                    return CardNames.queen;
                case "K":
                    return CardNames.king;
                case "A":
                    return CardNames.ace;
                default: throw new Exception("Not a valid card");
            }
        }
    }
}
