using System.Collections.Generic;

namespace President.API.Dtos
{
    public class ScoreDto
    {
        public string UserName { get; set; }
        public List<int> Points { get; set; }
        public int Total { get; set; }
    }
}
