﻿using System.Collections.Generic;
using System.Linq;

namespace President.BLL.Game
{
    public class DefaultGameLogic : IGameLogic
    {
        public void GiveRank(Dictionary<string, Hand> hands, string userName)
        {
            int allUsers = hands.Count;
            int finishedUsers = hands.Where((hand) => hand.Value.Cards.Count == 0).Count();
            switch (finishedUsers)
            {
                case (0):
                    return;
                case (1):
                    hands[userName].Rank = Rank.President;
                    hands[userName].Score.Add(5);
                    break;
                case (2):
                    hands[userName].Rank = Rank.VicePresident;
                    hands[userName].Score.Add(4);
                    break;
                default:
                    if (finishedUsers == allUsers - 1)
                    {
                        hands[userName].Rank = Rank.ViceScum;
                        hands[userName].Score.Add(2);
                    }
                    else
                    {
                        hands[userName].Rank = Rank.AverageJoe;
                        hands[userName].Score.Add(3);
                    }
                    break;
            }
            hands[userName].Active = false;
        }

        public bool IsGameStuck(Card AtTop, Dictionary<string, Game.Hand> Hands)
        {
            if (AtTop != null && AtTop.CardName == CardNames.ace) return true; // if the top card is ace
            if (Hands.Where(hand => hand.Value.Active).Count() == 1) return true; // if all except one person passed

            return false; // else game is not stuck
        }

        public bool IsValidMove(Card AtTop, Card FromHand)
        {
            if (AtTop == null) return true;
            return (AtTop.CardName < FromHand.CardName);
        }
    }
}
