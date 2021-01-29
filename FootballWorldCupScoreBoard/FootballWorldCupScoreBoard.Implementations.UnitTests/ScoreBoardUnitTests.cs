using FluentAssertions;
using FootballWorldCupScoreBoard.Implementations.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FootballWorldCupScoreBoard.Implementations.UnitTests
{
    [Trait("Type", "Unit")]
    public class ScoreBoardUnitTests
    {
        #region StartMatch
        [Fact]
        public void StartMatch_ShouldThrowArgumentException_WhenAwayTeamIsNull()
        {
            var scoreBoard = new ScoreBoard();
            Action action = () => scoreBoard.StartMatch(It.IsAny<Team>(), null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void StartMatch_ShouldThrowArgumentException_WhenHomeTeamIsNull()
        {
            var scoreBoard = new ScoreBoard();
            Action action = () => scoreBoard.StartMatch(null, It.IsAny<Team>());
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void StartMatch_ScoreBoardHasAtLeastOneMatch_WhenStartAMatchFirstTime()
        {
            var homeTeam = new Team { Name = "Manchester United" };
            var awayTeam = new Team { Name = "Chelsea" };

            var scoreBoard = new ScoreBoard();

            scoreBoard.StartMatch(homeTeam, awayTeam);

            Assert.Single(scoreBoard.Matches);
        }

        [Fact]
        public void StartMatch_ScoreBoardMatchesShouldNotBeEmpty_WhenExistsAnyMatch()
        {
            var homeTeam = new Team { Name = "Liverpool" };
            var awayTeam = new Team { Name = "Everton" };

            var scoreBoard = new ScoreBoard();

            scoreBoard.StartMatch(homeTeam, awayTeam);

            scoreBoard.Matches.Should().NotBeNullOrEmpty();
        }
        #endregion

        #region FinishMatch
        [Fact]
        public void FinishMatch_ShouldThrowArgumentException_WhenMatchFinishIsNull()
        {
            var scoreBoard = new ScoreBoard();
            Action action = () => scoreBoard.FinishMatch(It.IsAny<Models.Match>());
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FinishMatch_ScoreBoardMatchesShouldBeTheSame_WhenAMatchCanNotBeNotFound()
        {
            var scoreBoard = new ScoreBoard
            {
                Matches = MatchesGenerator()
            };

            var matchFinished = new Models.Match
            {
                Id = 10,
                HomeTeam = new Team { Name = "Luton Town" },
                AwayTeam = new Team { Name = "Leicester City" },
                Score = new Score { HomeScore = 0, AwayScore = 4 }
            };

            var matchesBeforeFinishMatch = scoreBoard.Matches.Count();

            scoreBoard.FinishMatch(matchFinished);

            scoreBoard.Matches.Should().HaveCount(matchesBeforeFinishMatch);
        }

        [Fact]
        public void FinishMatch_ScoreBoardMatchesShouldHaveALessMatch_WhenAMatchIsFinished()
        {
            var scoreBoard = new ScoreBoard
            {
                Matches = MatchesGenerator()
            };

            var matchFinished = new Models.Match
            {
                Id = 3,
                HomeTeam = new Team { Name = "Watford" },
                AwayTeam = new Team { Name = "Wigan Athletic" },
                Score = new Score { HomeScore = 0, AwayScore = 4 }
            };

            var matchesBeforeFinishMatch = scoreBoard.Matches.Count();

            scoreBoard.FinishMatch(matchFinished);

            scoreBoard.Matches.Should().HaveCount(matchesBeforeFinishMatch - 1);
        }
        #endregion

        #region UpdateMatch
        [Fact]
        public void UpdateMatch_ShouldThrowArgumentException_WhenMatchFinishIsNull()
        {
            var scoreBoard = new ScoreBoard();
            Action action = () => scoreBoard.UpdateMatch(null, It.IsAny<int>(), It.IsAny<int>());
            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null, 1, 1)]
        [InlineData(2, null, 2)]
        [InlineData(2, 2, 4)]
        [InlineData(0, 2, 2)]
        [InlineData(null, null, 0)]
        public void UpdateMatch_UpdateScoreOfMatchAndTotalScore_WhenHomeAndAwayScoreIsSent(int? homeScore, int? awayScore , int expected)
        {
            var scoreBoard = new ScoreBoard
            {
                Matches = MatchesGenerator()
            };

            var matchToUpdate = new Models.Match
            {
                Id = 5,
                HomeTeam = new Team { Name = "Birmingham City" },
                AwayTeam = new Team { Name = "Plymouth Argyle" },
                Score = new Score ()
            };

            scoreBoard.UpdateMatch(matchToUpdate, homeScore, awayScore);

            var matchUpdated = scoreBoard.Matches.Where(c => c.Id.Equals(5)).SingleOrDefault();

            Assert.Equal(expected, matchUpdated.Score.TotalScore);
        }
        #endregion

        #region GetMatchesSummary
        [Theory]
        [InlineData(4, 0)]
        [InlineData(1, 1)]
        [InlineData(3, 2)]
        [InlineData(2, 3)]
        [InlineData(5, 4)]
        public void GetMatchesSummary_ScoreBoardMatchesReturnMatchesOrdered_WhenSummaryIsDemanded(int matchId, int expected)
        {
            var scoreBoard = new ScoreBoard
            {
                Matches = MatchesGenerator()
            };

            var result = scoreBoard.GetMatchesSummary();

            var matchOrderedIndex = result.IndexOf(result.Where(c => c.Id.Equals(matchId)).SingleOrDefault());

            Assert.Equal(expected, matchOrderedIndex);
        }
        #endregion

        #region Matches Collection
        private List<Models.Match> MatchesGenerator()
        {
            var matches = new List<Models.Match>();

            var matchA = new Models.Match
            {
                Id = 1,
                HomeTeam = new Team { Name = "Aston Villa" },
                AwayTeam = new Team { Name = "Fulham" },
                Score = new Score { HomeScore = 2, AwayScore = 3}
            };

            var matchB = new Models.Match
            {
                Id = 2,
                HomeTeam = new Team { Name = "Porstmouth" },
                AwayTeam = new Team { Name = "Liverpool" },
                Score = new Score { HomeScore = 2, AwayScore = 2 }
            };

            var matchC = new Models.Match
            {
                Id = 3,
                HomeTeam = new Team { Name = "Watford" },
                AwayTeam = new Team { Name = "Wigan Athletic" },
                Score = new Score { HomeScore = 0, AwayScore = 4 }
            };

            var matchD = new Models.Match
            {
                Id = 4,
                HomeTeam = new Team { Name = "Bolton Wanderers" },
                AwayTeam = new Team { Name = "Tottenham Hotspur" },
                Score = new Score { HomeScore = 1, AwayScore = 4 }
            };

            var matchE = new Models.Match
            {
                Id = 5,
                HomeTeam = new Team { Name = "Birmingham City" },
                AwayTeam = new Team { Name = "Plymouth Argyle" },
                Score = new Score()
            };

            matches.Add(matchA);
            matches.Add(matchB);
            matches.Add(matchC);
            matches.Add(matchD);
            matches.Add(matchE);

            return matches;
        }
        #endregion
    }
}
