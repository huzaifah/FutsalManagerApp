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
using System.Globalization;

namespace FutsalManager.Droid
{
    [Activity(Label = "Player Profile", ScreenOrientation = ScreenOrientation.Landscape)]
    public class PlayerDetailActivity : Activity, DatePickerDialog.IOnDateSetListener
    {
        PlayerDto _player;

        EditText _nameEditText;
        EditText _ageEditText;
        TextView _birthDateTextView;
        Button _pickDateButton;
        Spinner _positionSpinner;
        ArrayAdapter _positionAdapter;

        const int DATE_DIALOG_ID = 0;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.PlayerDetail);

            _nameEditText = FindViewById<EditText>(Resource.Id.nameEditText);
            _ageEditText = FindViewById<EditText>(Resource.Id.ageEditText);
            _birthDateTextView = FindViewById<TextView>(Resource.Id.birthDateTextView);
            _pickDateButton = FindViewById<Button>(Resource.Id.pickDatePlayerButton);
            _positionSpinner = FindViewById<Spinner>(Resource.Id.positionSpinner);

            _positionSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(_positionSpinner_ItemSelected);
            _positionAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.position_array, Android.Resource.Layout.SimpleSpinnerItem);
            _positionAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            _positionSpinner.Adapter = _positionAdapter;

            _birthDateTextView.Text = DateTime.Now.ToString("dd/MM/yyyy");

            if (Intent.HasExtra("playerId"))
            {
                var playerId = Intent.GetStringExtra("playerId");
                _player = AppData.Service.GetPlayerById(playerId);
            }
            else
                _player = new PlayerDto();

            _pickDateButton.Click += delegate { ShowDialog(DATE_DIALOG_ID); };

            UpdateUI();
        }

        private void _positionSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var positionSpinner = sender as Spinner;
        }
              
        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case DATE_DIALOG_ID:
                    return new DatePickerDialog(this, OnDateSetEvent, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
            }
            return null;
        }

        private void UpdateUI()
        {
            _nameEditText.Text = _player.Name;
            //_positionEditText.Text = _player.Position;
            _ageEditText.Text = _player.Age.ToString();
            _birthDateTextView.Text = _player.BirthDate.ToString("dd/MM/yyyy");

            var position = _positionAdapter.GetPosition(_player.Position);
            _positionSpinner.SetSelection(position);
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
            //_player.Position = _positionEditText.Text;
            _player.BirthDate = DateTime.ParseExact(_birthDateTextView.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            _player.Position = _positionSpinner.SelectedItem.ToString();

            AppData.Service.AddEditPlayer(_player);
            var toast = Toast.MakeText(this, String.Format("{0} saved.", _player.Name), ToastLength.Short);
            toast.Show();
            Finish();
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {            
        }

        // the event received when the user "sets" the date in the dialog
        private void OnDateSetEvent(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            _birthDateTextView.Text = e.Date.ToString("dd/MM/yyyy");
        }
    }
}