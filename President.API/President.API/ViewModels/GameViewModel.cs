using President.BLL.Dtos;
using System.Collections.Generic;

namespace President.API.ViewModels
{
    public class GameViewModel
    {
        public List<CardDto> Cards { get; set; }
        public string OwnRank { get; set; }
        public List<HandStatusDto> Hands { get; set; }
        public string NextUser { get; set; }
        public int Round { get; set; }
    }
}
