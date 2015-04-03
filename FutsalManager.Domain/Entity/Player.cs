using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Entity
{
    public class Player
    {
        TextInfo myTI = new CultureInfo("en-US").TextInfo;

        public string Id { get; set; }
        
        private string _name;
        public string Name
        {
            get {
                if (!String.IsNullOrEmpty(_name))
                    return myTI.ToTitleCase(_name);
                else
                    return String.Empty; 
            }
            set { _name = value; }
        }        

        public string Position { get; set; }
        public DateTime BirthDate { get; set; }
        public string TeamId { get; set; }
        public string TournamentId { get; set; }
        public string Attendance { get; set; }
        public bool Paid { get; set; }
        public long TotalGoals { get; set; }

        public Player()
        {

        }

        public Player(string name)
        {
            Name = name;
        }

        public Player(string name, string teamId, string tournamentId)
        {
            Name = name;
            TeamId = teamId;
            TournamentId = tournamentId;
        }
    }
}
