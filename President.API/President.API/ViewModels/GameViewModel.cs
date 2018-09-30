using President.API.Dtos;
using System.Collections.Generic;

namespace President.API.ViewModels
{
    public class GameViewModel
    {
        public List<CardDto> Cards { get; set; }
        public Dictionary<string, int> Hands { get; set; }
    }
}
