using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FutsalManager.Domain.Entity
{
    public class TeamStanding
    {
        public Team Team { get; set; }
        public int TotalMatches { get; set; }
        public int TotalWins { get; set; }
        public int TotalDraws { get; set; }
        public int TotalLoses { get; set; }
        public int TotalScores { get; set; }
        public int TotalConceded { get; set; }
        public int GoalDifference { get; set; }
        public int TotalPoints { get; set; }
    }
}