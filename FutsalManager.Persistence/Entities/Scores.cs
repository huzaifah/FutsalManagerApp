
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Persistence.Entities
{
    public class Scores
    {
        public Guid TournamentId { get; set; }
        public Guid MatchId { get; set; }
        public Guid TeamId { get; set; }
        public Guid PlayerId { get; set; }
        public string Remark { get; set; }
    }
}
