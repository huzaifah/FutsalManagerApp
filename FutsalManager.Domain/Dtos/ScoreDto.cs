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

namespace FutsalManager.Domain.Dtos
{
    public class ScoreDto
    {
        public string TournamentId { get; set; }
        public string MatchId { get; set; }
        public string TeamId { get; set; }
        public string ScorerId { get; set; }
        public string Remark { get; set; }
    }
}