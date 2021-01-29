using System;
using System.Collections.Generic;
using System.Text;

namespace FootballWorldCupScoreBoard.Implementations.Models
{
    public class Match
    {
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }

        public Score Score { get; set; }
    }
}
