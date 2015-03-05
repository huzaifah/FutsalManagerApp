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
using FutsalManager.Domain.Dtos;
using FutsalManager.Droid.Adapters;

namespace FutsalManager.Droid
{
    [Activity(Label = "Player Profile", ScreenOrientation = ScreenOrientation.Landscape)]
    public class PlayerDetailActivity : Activity
    {
        PlayerDto _player;

        EditText _nameEditText;
        EditText _positionEditText;
        EditText _ageEditText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.PlayerDetail);

            _nameEditText = FindViewById<EditText>(Resource.Id.nameEditText);
            _positionEditText = FindViewById<EditText>(Resource.Id.positionEditText);
            _ageEditText = FindViewById<EditText>(Resource.Id.ageEditText);

            if (Intent.HasExtra("playerId"))
            {
                var playerId = Intent.GetStringExtra("playerId");
                _player = AppData.Service.GetPlayerById(playerId);
            }
            else
                _player = new PlayerDto();

            UpdateUI();
        }

        private void UpdateUI()
        {
            _nameEditText.Text = _player.Name;
            _positionEditText.Text = _player.Position;
            _ageEditText.Text = "??";
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.PlayerDetailMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            base.OnPrepareOptionsMenu(menu);

            if (String.IsNullOrEmpty(_player.Id))
            {
                var item = menu.FindItem(Resource.Id.actionDeletePlayer);
                item.SetEnabled(false);
            }

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.actionSavePlayer:
                    SavePlayer();
                    return true;
                case Resource.Id.actionDeletePlayer:
                    DeletePlayer();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }            
        }

        private void DeletePlayer()
        {
            AlertDialog.Builder alertConfirm = new AlertDialog.Builder(this);
            alertConfirm.SetCancelable(false);
            alertConfirm.SetPositiveButton("OK", ConfirmDelete);
            alertConfirm.SetNegativeButton("Cancel", delegate { });
            alertConfirm.SetMessage(String.Format("Are you sure you want to delete {0}?", _player.Name));
            alertConfirm.Show();
        }

        protected void ConfirmDelete(object sender, EventArgs e)
        {
            //AppData.Service.
            Toast toast = Toast.MakeText(this, String.Format("{0} deleted.", _player.Name), ToastLength.Short);
            toast.Show();
            Finish();
        }

        private void SavePlayer()
        {
            if (String.IsNullOrEmpty(_player.Id))
                _player.Id = Guid.NewGuid().ToString();

            _player.Name = _nameEditText.Text;
            _player.Position = _positionEditText.Text;

            AppData.Service.AddEditPlayer(_player);
            var toast = Toast.MakeText(this, String.Format("{0} saved.", _player.Name), ToastLength.Short);
            toast.Show();
            Finish();
        }
    }
}