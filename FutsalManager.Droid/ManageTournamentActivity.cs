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

namespace FutsalManager.Droid
{
    [Activity(Label = "Manage Tournament")]
    public class ManageTournamentActivity : Activity
    {
        public string TournamentId { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (Intent.HasExtra("tournamentId"))
            {
                TournamentId = Intent.GetStringExtra("tournamentId");
            }

            // Create your application here
            SetContentView(Resource.Layout.ManageTournament);
        }
    }
}