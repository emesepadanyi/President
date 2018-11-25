﻿using System.Collections.Generic;

namespace President.API.Game
{
    public class Score
    {
        public List<int> Points { get; set; } = new List<int>();
        public int Total { get {
                int sum = 0;
                Points.ForEach(point => sum += point);
                return sum;
            }
        }

        public void Add(int point)
        {
            Points.Add(point);
        }
    }
}
