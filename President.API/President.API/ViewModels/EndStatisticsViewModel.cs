using President.API.Dtos;
using President.DAL.Entities;
using System.Collections.Generic;

namespace President.API.ViewModels
{
    public class EndStatisticsViewModel
    {
        public List<ScoreDto> ScoreCard { get; set; }
        public PlayerStatistics Stats { get; set; }
    }
}
