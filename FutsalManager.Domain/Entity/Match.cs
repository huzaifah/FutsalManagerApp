using FutsalManager.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Entity
{
    public class Match
    {
        public string Id { get; set; }
        public string TournamentId { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public bool IsCompleted { get; set; }

        /*
        private List<Score> Scores { get; set; }

        private IReadOnlyList<Score> ScoreList
        {
            get
            {
                return Scores;
            }
        }
        */

        public Match() { }

        public Match(Team home, Team away)
        {
            HomeTeam = home;
            AwayTeam = away;

            //Scores = new List<Score>();
        }

        /*
        public void AddScore(Team scoredTeam, Player player, string remark = "")
        {
            Scores.Add(
                new Score 
                {
                    Team = scoredTeam,
                    Scorer = player,
                    Remark = remark
                });
        }
         */
    }
}
