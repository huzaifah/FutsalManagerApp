using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Entity
{
    public class Team
    {
        public string Id { get; set; }
        public string TournamentId { get; set; }
        public string Name { get; set; }
        
        //public IList<Player> Players { get; set; }

        public Team()
        {
        }

        public Team(string name)
        {
            Name = name;

            //Players = new List<Player>();
        }
    }
}
