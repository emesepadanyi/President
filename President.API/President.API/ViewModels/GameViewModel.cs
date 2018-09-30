using President.API.Game;
using President.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.ViewModels
{
    public class GameViewModel
    {
        public List<Card> Cards { get; set; }
        public Dictionary<string, int> Hands { get; set; }
    }
}
