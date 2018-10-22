using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.Game
{
    public interface IGameLogic
    {
        bool IsValidMove(Card AtTop, Card FromHand);
        bool IsGameStuck(Card AtTop);
    }
}
