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
using FutsalManager.Droid.Adapters;
using FutsalManager.Domain.Entity;

namespace FutsalManager.Droid
{
    [Activity(ScreenOrientation = ScreenOrientation.Landscape)]
    public class TournamentSetupActivity : Activity
    {
        TextView _tournamentDateTextView;
        Button _pickDateButton;
        Tournament tournament = new Tournament();
        bool errors = false;
        const int DATE_DIALOG_ID = 0;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.TournamentSetup);

            _tournamentDateTextView = FindViewById<TextView>(Resource.Id.tournamentDateEditText);
            _pickDateButton = FindViewById<Button>(Resource.Id.pickDateTournamentButton);

            _pickDateButton.Click += delegate { ShowDialog(DATE_DIALOG_ID); };

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

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
        }

        // the event received when the user "sets" the date in the dialog
        private void OnDateSetEvent(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            if (DateTime.Now > e.Date)
            {
                _tournamentDateTextView.Error = "Tournament date cannot be earlier than today's date";
                errors = true;
            }
            else
            {
                _tournamentDateTextView.Error = null;
                _tournamentDateTextView.Text = e.Date.ToString("dd/MM/yyyy");
                tournament.Date = e.Date;
                errors = false;
            }
          
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.TournamentSetupMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.actionSaveTournament:
                    CreateTournament();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void CreateTournament()
        {
            if (!errors)
            { 
                tournament.MaxPlayerPerTeam = 6;
                tournament.TotalTeam = 4;

                AppData.Service.AddEditTournament(tournament);
                var toast = Toast.MakeText(this, String.Format("New tournament created."), ToastLength.Short);
                toast.Show();
                Finish();
            }
        }
    }
}