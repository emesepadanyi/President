using President.API.Dtos;
using President.API.Game;
using System.Collections.Generic;

namespace President.API.ViewModels
{
    public class NewRoundViewModel
    {
        public bool Wait { get; set; }
        public List<CardDto> Cards { get; set; }
        public List<CardDto> SwitchedCards { get; set; }
        public string OwnRank { get; set; }
        public List<ScoreDto> ScoreCard { get; set; }
    }
}
