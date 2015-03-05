using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Persistence.Entities
{
    public class Players
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Name { get; set; }
        //public string TeamId { get; set; }
        //public string TournamentId { get; set; }
    }
}
