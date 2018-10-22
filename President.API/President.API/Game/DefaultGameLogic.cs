using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.Game
{
    public class DefaultGameLogic : IGameLogic
    {
        public bool IsValidMove(Card AtTop, Card FromHand)
        {
            if (AtTop == null) return true;
            return (AtTop.CardName <= FromHand.CardName) ? true : false;
        }
    }
}
