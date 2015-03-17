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
    [Activity(Label = "Administration", ScreenOrientation = ScreenOrientation.Landscape)]
    public class AdminActivity : Activity
    {
        Button _playerButton;
        Button _tournamentButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.AdminMenu);

            _playerButton = FindViewById<Button>(Resource.Id.playerButton);
            _tournamentButton = FindViewById<Button>(Resource.Id.tournamentButton);

            _playerButton.Click += delegate { StartActivity(typeof(PlayerListActivity)); };
        }
    }
}