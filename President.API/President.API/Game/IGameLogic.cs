using System.Collections.Generic;

namespace President.API.Game
{
    public interface IGameLogic
    {
        bool IsValidMove(Card AtTop, Card FromHand);
        bool IsGameStuck(Card AtTop, Dictionary<string, Game.Hand> Hands);
        void GiveRank(Dictionary<string, Hand> hands, string userName);
    }
}
