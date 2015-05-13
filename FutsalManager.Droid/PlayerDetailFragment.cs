using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using FutsalManager.Domain.Dtos;
using FutsalManager.Droid.Adapters;
using System.Globalization;

namespace FutsalManager.Droid
{
    public class PlayerDetailFragment : Fragment, DatePickerDialog.IOnDateSetListener
    {
        PlayerDto _player;
        DateTime selectedDate = DateTime.Now;

        EditText _nameEditText;
        EditText _ageEditText;
        TextView _birthDateTextView;
        Button _pickDateButton;
        Spinner _positionSpinner;
        ArrayAdapter _positionAdapter;
        Button _savePlayerButton;
        Button _deletePlayerButton;

        const int DATE_DIALOG_ID = 0;

        public static PlayerDetailFragment NewInstance(string playerId)
        {
            var playerDetailFragment = new PlayerDetailFragment { Arguments = new Bundle() };
            playerDetailFragment.Arguments.PutString("player_id", playerId);
            
            return playerDetailFragment;
        }

        public string PlayerId {
            get
            {
                return Arguments.GetString("player_id", "");
            }        
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PlayerDetail, null);

            _nameEditText = view.FindViewById<EditText>(Resource.Id.nameEditText);
            _ageEditText = view.FindViewById<EditText>(Resource.Id.ageEditText);
            _birthDateTextView = view.FindViewById<TextView>(Resource.Id.birthDateTextView);
            _pickDateButton = view.FindViewById<Button>(Resource.Id.pickDatePlayerButton);
            _savePlayerButton = view.FindViewById<Button>(Resource.Id.savePlayerButton);
            _deletePlayerButton = view.FindViewById<Button>(Resource.Id.deletePlayerButton);
            _positionSpinner = view.FindViewById<Spinner>(Resource.Id.positionSpinner);

            //_positionSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(_positionSpinner_ItemSelected);
            _positionAdapter = ArrayAdapter.CreateFromResource(Activity, Resource.Array.position_array, Android.Resource.Layout.SimpleSpinnerItem);
            _positionAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            _positionSpinner.Adapter = _positionAdapter;

            _pickDateButton.Click += delegate {
                var dialog = new DatePickerDialogFragment(Activity, selectedDate, this);
                dialog.Show(FragmentManager, null);
            };
            _savePlayerButton.Click += _savePlayerButton_Click;
            _deletePlayerButton.Click += _deletePlayerButton_Click;
            _deletePlayerButton.Enabled = false;

            if (!String.IsNullOrEmpty(PlayerId))
                _player = AppData.Service.GetPlayerById(PlayerId);

            UpdateUI();

            return view;
        }

        void _deletePlayerButton_Click(object sender, EventArgs e)
        {
            DeletePlayer();
        }

        void _savePlayerButton_Click(object sender, EventArgs e)
        {
            SavePlayer();
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
            selectedDate = date;
            _birthDateTextView.Text = date.ToString("dd/MM/yyyy");
        }

        private void UpdateUI()
        {
            if (_player == null)
                return;

            _nameEditText.Text = _player.Name;
            _ageEditText.Text = _player.Age.ToString();
            _birthDateTextView.Text = _player.BirthDate.ToString("dd/MM/yyyy");
            selectedDate = _player.BirthDate;
            _deletePlayerButton.Enabled = true;

            var position = _positionAdapter.GetPosition(_player.Position);
            _positionSpinner.SetSelection(position);
        }

        private void ClearDetailFragment()
        {
            var details = FragmentManager.FindFragmentById(Resource.Id.fragmentContainer) as PlayerDetailFragment;

            if (details != null)
            {
                // clear fragment
                // execute a transaction, replacing any existing fragment with this one inside the frame
                var ft = FragmentManager.BeginTransaction();
                ft.Remove(details);
                ft.SetTransition(FragmentTransit.FragmentFade);
                ft.Commit();
            }
        }

        private void DeletePlayer()
        {
            AlertDialog.Builder alertConfirm = new AlertDialog.Builder(Activity);
            alertConfirm.SetCancelable(false);
            alertConfirm.SetPositiveButton("OK", ConfirmDelete);
            alertConfirm.SetNegativeButton("Cancel", delegate { });
            alertConfirm.SetMessage(String.Format("Are you sure you want to delete {0}?", _player.Name));
            alertConfirm.Show();
        }

        protected void ConfirmDelete(object sender, EventArgs e)
        {
            AppData.Service.DeletePlayer(_player.Id);
            Toast toast = Toast.MakeText(Activity, String.Format("{0} deleted.", _player.Name), ToastLength.Short);
            toast.Show();

            RefreshPlayerList();
            ClearDetailFragment();
        }

        private void SavePlayer()
        {
            if (_player == null)
                _player = new PlayerDto();

            if (String.IsNullOrEmpty(_player.Id))
                _player.Id = Guid.NewGuid().ToString();

            _player.Name = _nameEditText.Text;
            //_player.Position = _positionEditText.Text;

            _player.BirthDate = selectedDate;            
            _player.Position = _positionSpinner.SelectedItem.ToString();

            AppData.Service.AddEditPlayer(_player);
            var toast = Toast.MakeText(Activity, String.Format("{0} saved.", _player.Name), ToastLength.Short);
            toast.Show();

            RefreshPlayerList();
        }

        private void RefreshPlayerList()
        {
            AppData.Service.RefreshPlayerCache();

            var details = FragmentManager.FindFragmentById(Resource.Id.list_fragment) as PlayerTournamentFragment;

            if (details != null || details.playerAdapter != null)
            {
                // refresh player cache
                details.playerAdapter.NotifyDataSetChanged();
            }
        }
    }
}