using President.API.Dtos;
using System.Collections.Generic;

namespace President.API.ViewModels
{
    public class GameViewModel
    {
        public List<CardDto> Cards { get; set; }
        public List<Hand> Hands { get; set; }
        public string NextUser { get; set; }
    }
}
