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
using FutsalManager.Droid.Adapters;
using Android.Content.PM;

namespace FutsalManager.Droid
{
    [Activity(Label = "Manage Players", ConfigurationChanges = (Android.Content.PM.ConfigChanges.Orientation |
    Android.Content.PM.ConfigChanges.ScreenSize), ScreenOrientation = ScreenOrientation.Landscape, Icon = "@drawable/icon")]
    public class PlayerListActivity : Activity
    {
        ListView _playerTourView;
        PlayerListViewAdapter _playerAdapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.PlayerTourList);

            _playerTourView = FindViewById<ListView>(Resource.Id.playerTourView);
            _playerAdapter = new PlayerListViewAdapter(this);
            _playerTourView.Adapter = _playerAdapter;
            _playerTourView.ItemClick += PlayerTourView_ItemClick;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.PlayerListViewMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.newPlayer:
                    StartActivity(typeof(PlayerDetailActivity));
                    return true;
                case Resource.Id.actionPlayerRefresh:
                    AppData.Service.RefreshPlayerCache();
                    _playerAdapter.NotifyDataSetChanged();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }            
        }

        protected void PlayerTourView_ItemClick(object sender, ListView.ItemClickEventArgs e)
        {
            var player = AppData.Service.GetPlayerByItemId(e.Id);
            //Console.WriteLine("Player ", player.Name);

            var playerDetailIntent = new Intent(this, typeof(PlayerDetailActivity));
            playerDetailIntent.PutExtra("playerId", player.Id);
            StartActivity(playerDetailIntent);
        }

        protected override void OnResume()
        {
            base.OnResume();
            AppData.Service.RefreshPlayerCache();
            _playerAdapter.NotifyDataSetChanged();

        }

    }
}