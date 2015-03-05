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
using Android.Content.PM;

namespace FutsalManager.Droid
{
    [Activity(MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape)]
    public class TournamentSetupActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.TournamentSetup);
            // Create your application here
        }
    }
}