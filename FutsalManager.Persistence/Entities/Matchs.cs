using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Persistence.Entities
{
    public class Matchs
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public Guid HomeTeam { get; set; }
        public Guid AwayTeam { get; set; }
        public bool IsCompleted { get; set; }
    }
}
