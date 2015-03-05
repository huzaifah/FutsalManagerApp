using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Dtos
{
    public class MatchDto
    {
        public string Id { get; set; }
        public string TournamentId { get; set; }
        public TeamDto HomeTeam { get; set; }
        public TeamDto AwayTeam { get; set; }
        public bool IsCompleted { get; set; }
    }
}
