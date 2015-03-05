using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Dtos
{
    public class TeamDto
    {
        public string Id { get; set; }
        public string TournamentId { get; set; }
        public string Name { get; set; }
    }
}
