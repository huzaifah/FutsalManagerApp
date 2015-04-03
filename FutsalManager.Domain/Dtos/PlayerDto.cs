using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Dtos
{
    public class PlayerDto
    {
        TextInfo myTI = new CultureInfo("en-US").TextInfo;

        public long ListItemId { get; set; }
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

        public int Age
        {
            get 
            {
                if (BirthDate == null)
                    return 0;

                return ((DateTime.Now - BirthDate).Days) / 365; 
            }
        }
        
    }
}
