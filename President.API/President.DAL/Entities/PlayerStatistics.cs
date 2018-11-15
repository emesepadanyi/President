using System.Collections.Generic;

namespace President.DAL.Entities
{
    public class PlayerStatistics
    {
        public int ID { get; set; }
        public User User { get; set; }
        public int GamesPlayed { get; set; }
        public int SumPointsEarned { get; set; }
        public int TimeSpentPlaying { get; set; }
        public int TimesWon { get; set; }
        public ICollection<PlayerGameStatistics> PlayerGameStatistics { get; } = new List<PlayerGameStatistics>();
    }
}
