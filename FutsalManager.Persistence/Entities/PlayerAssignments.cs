using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Persistence.Entities
{
    public class PlayerAssignments
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public Guid PlayerId { get; set; }
        public Guid TeamId { get; set; }
        public Guid TournamentId { get; set; }
        public string Attendance { get; set; }
        public bool Paid { get; set; }
    }
}
