using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.API.Dtos
{
    public class ScoreDto
    {
        public string UserName { get; set; }
        public List<int> Points { get; set; }
        public int Total { get; set; }
    }
}
