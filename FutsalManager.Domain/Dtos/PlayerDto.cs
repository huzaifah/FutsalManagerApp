using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Dtos
{
    public class PlayerDto
    {
        public long ListItemId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string TeamId { get; set; }
        public string TournamentId { get; set; }
        public string Position { get; set; }
    }
}
