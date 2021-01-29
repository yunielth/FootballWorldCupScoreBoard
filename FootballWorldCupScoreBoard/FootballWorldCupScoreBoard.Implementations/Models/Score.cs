using System;
using System.Collections.Generic;
using System.Text;

namespace FootballWorldCupScoreBoard.Implementations.Models
{
    public class Score
    {
        public int HomeScore { get; set; } = default;
        public int AwayScore { get; set; } = default;

        public int TotalScore
        { 
            get { return HomeScore + AwayScore; }
        }
    }
}
