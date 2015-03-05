using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Entity
{
    public class Score
    {
        public string TournamentId { get; set; }
        public string MatchId { get; set; }
        public Team Team { get; set; }
        public Player Scorer { get; set; }
        public string Remark { get; set; }
    
        public Score()
        {

        }
    }
}
