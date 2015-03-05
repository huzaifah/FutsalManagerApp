using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Entity
{
    public class Tournament
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public int TotalTeam { get; set; }
        public int MaxPlayerPerTeam { get; set; }
        
        /*        
        private List<Team> Teams { get; set; }

        public IReadOnlyList<Team> TeamList
        {
            get
            {
                return Teams;
            }
        }

        private Dictionary<string, Match> Matches { get; set; }

        public IReadOnlyDictionary<string, Match> MatchList
        {
            get
            {
                return Matches;
            }
        }
         */

        public Tournament(DateTime tournamentDate, int totalTeam, int maxPlayerPerTeam)
        {
            Date = tournamentDate;
            TotalTeam = totalTeam;
            MaxPlayerPerTeam = maxPlayerPerTeam;
            //Teams = new List<Team>();
            //Matches = new Dictionary<string, Match>();
        }

        public Tournament()
        {
            //Teams = new List<Team>();
            //Matches = new Dictionary<string, Match>();
        }

        /*
        public void AddTeam(Team team)
        {
            Teams.Add(team);
        }

        public void AddMatch(Match match)
        {
            var key = String.Format("{0}v{1}", match.HomeTeam.Name, match.AwayTeam.Name);
            Matches.Add(key, match);
        }
         */
    }
}
