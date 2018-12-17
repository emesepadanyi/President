using President.BLL.Game;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace President.Tests.Game
{
    public class ScoreTests
    {
        [Fact]
        public void AddPoints_ShouldAddPoints()
        {
            var score = new Score();

            score.Add(5);

            Assert.Single(score.Points);
        }

        [Fact]
        public void TotalPoints_ShouldSumUpPoints()
        {
            var score = new Score();

            score.Add(5);
            score.Add(4);
            score.Add(3);

            Assert.Equal(12, score.Total);
        }

        [Fact]
        public void TotalPoints_ShouldReturn0()
        {
            var score = new Score();

            Assert.Equal(0, score.Total);
        }
    }
}
