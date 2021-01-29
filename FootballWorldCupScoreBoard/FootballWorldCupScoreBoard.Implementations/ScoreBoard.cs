using FootballWorldCupScoreBoard.Implementations.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballWorldCupScoreBoard.Implementations
{
    public class ScoreBoard
    {
        public List<Match> Matches { get; set; } = new List<Match>();

        public void StartMatch(Team homeTeam, Team awayTeam)
        {
            if (homeTeam == null) { throw new ArgumentNullException(nameof(homeTeam)); }
            if (awayTeam == null) { throw new ArgumentNullException(nameof(awayTeam)); }

            var match = new Match
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                Score = new Score(),
                Id = Matches.Count + 1
            };

            Matches.Add(match);
        }

        public void FinishMatch(Match match)
        {
            if(match == null) { throw new ArgumentNullException(nameof(match)); }

            if (Matches.Any())
            {
                var matchToFinish = Matches.Where(c => c.Id.Equals(match.Id)).SingleOrDefault();

                if (matchToFinish != null)
                {
                    Matches.Remove(matchToFinish);
                }
            }

        }

        public void UpdateMatch(Match match, int? homeScore, int? awayScore)
        {
            if (match == null) { throw new ArgumentNullException(nameof(match)); }

            if (Matches.Any())
            {
                var matchToUpdate = Matches.Where(c => c.Id.Equals(match.Id)).SingleOrDefault();

                if (matchToUpdate != null)
                {
                    if (homeScore.HasValue) 
                    { 
                        matchToUpdate.Score.HomeScore = homeScore.Value; 
                    }
                    if (awayScore.HasValue) 
                    {
                        matchToUpdate.Score.AwayScore = awayScore.Value;
                    }
                }

            }

        }

        public List<Match> GetMatchesSummary()
        {
            var orderedMatches = new List<Match>();

            if (Matches.Any())
            {
                orderedMatches = Matches.OrderByDescending(c => c.Score.TotalScore).ThenByDescending(c=> c.Id).ToList();
            }

            return orderedMatches;
        }


    }
}
