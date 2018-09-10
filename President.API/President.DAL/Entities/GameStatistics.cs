using System.Collections.Generic;

namespace President.DAL.Entities
{
    public class GameStatistics
    {
        public int ID { get; set; }
        public double AvgCardsInHandsWhenWinning { get; set; }
        public int DifferenceBWNBestAndWorstPlayer { get; set; }
        public int GameID { get; set; }
        public Game Game { get; set; } // navigation prop
        public int TimeSpentPlayingSec { get; set; }

       // public ICollection<PlayerGameStatistics> playerGameStatistics { get; } = new List<PlayerGameStatistics>();
    }
}
