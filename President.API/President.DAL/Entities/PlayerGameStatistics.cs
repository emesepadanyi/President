using System.Collections.Generic;

namespace President.DAL.Entities
{
    public class PlayerGameStatistics
    {
        public int PlayerStatisticsId { get; set; }
        public PlayerStatistics PlayerStatistics { get; set; }

        public int GameStatisticsId { get; set; }
        public GameStatistics GameStatistics { get; set; }

        public ICollection<int> Points { get; } = new List<int>();

    }
}
