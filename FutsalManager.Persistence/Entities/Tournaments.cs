using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Persistence.Entities
{
    public class Tournaments
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int TotalTeam { get; set; }
        public int MaxPlayerPerTeam { get; set; }
        
    }
}
