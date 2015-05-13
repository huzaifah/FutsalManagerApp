using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

namespace FutsalManager.Droid
{
    [Activity(Label = "Super Bobai Cup", ScreenOrientation = ScreenOrientation.Landscape, MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button _newTournamentButton;
        Button _adminButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _newTournamentButton = FindViewById<Button>(Resource.Id.newTournamentButton);
            _newTournamentButton.Click += delegate { StartActivity(typeof(TournamentSetupActivity)); };

            _adminButton = FindViewById<Button>(Resource.Id.adminButton);
            _adminButton.Click += delegate {

                StartActivity(typeof(MasterDataActivity));
                //StartActivity(typeof(AdminActivity)); 
            
            };

        }  

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.masterData:
                    StartActivity(typeof(MasterDataActivity));
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }            
        }


    }
}

