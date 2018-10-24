using System.Collections.Generic;
using System.Linq;

namespace President.API.Game
{
    public class DefaultGameLogic : IGameLogic
    {
        public bool IsGameStuck(Card AtTop, Dictionary<string, Game.Hand> Hands)
        {
            if (AtTop != null && AtTop.CardName == CardNames.ace) return true; // if the top card is ace
            if (Hands.Where(hand => hand.Value.Active).Count() == 1) return true; // if all except one person passed

            return false; // else game is not stuck
        }

        public bool IsValidMove(Card AtTop, Card FromHand)
        {
            if (AtTop == null) return true;
            return (AtTop.CardName < FromHand.CardName) ? true : false;
        }
    }
}
