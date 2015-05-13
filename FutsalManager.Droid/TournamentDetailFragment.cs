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
using FutsalManager.Domain.Entity;
using FutsalManager.Droid.Adapters;
using FutsalManager.Domain.Exceptions;

namespace FutsalManager.Droid
{
    public class TournamentDetailFragment : Fragment, DatePickerDialog.IOnDateSetListener
    {
        Tournament _tournament;
        DateTime selectedDate = DateTime.Now;

        EditText _tournamentDateEditText;
        EditText _statusEditText;
        EditText _championEditText;
        Button _pickDateButton;
        Button _manageTournamentButton;
        Button _createTournamentButton;
        Button _startEndTournamentButton;
        Button _deleteTournamentButton;

        const int DATE_DIALOG_ID = 0;

        bool errors = false;

        public static TournamentDetailFragment NewInstance(string tournamentId)
        {
            var tournamentDetailFragment = new TournamentDetailFragment { Arguments = new Bundle() };
            tournamentDetailFragment.Arguments.PutString("tournament_id", tournamentId);

            return tournamentDetailFragment;
        }

        public string TournamentId
        {
            get
            {
                return Arguments.GetString("tournament_id", "");
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.TournamentSetup, null);

            _tournamentDateEditText = view.FindViewById<EditText>(Resource.Id.tournamentDateEditText);
            _statusEditText = view.FindViewById<EditText>(Resource.Id.statusEditText);
            _championEditText = view.FindViewById<EditText>(Resource.Id.championEditText);
            _pickDateButton = view.FindViewById<Button>(Resource.Id.pickDateTournamentButton);
            _manageTournamentButton = view.FindViewById<Button>(Resource.Id.manageTournamentButton);
            _createTournamentButton = view.FindViewById<Button>(Resource.Id.createTournamentButton);
            _startEndTournamentButton = view.FindViewById<Button>(Resource.Id.startEndTournamentButton);
            _deleteTournamentButton = view.FindViewById<Button>(Resource.Id.deleteTournamentButton);
                        
            _createTournamentButton.Click += delegate { CreateTournament(); };            
            _pickDateButton.Click += delegate { 
                var dialog = new DatePickerDialogFragment(Activity, selectedDate, this);
                dialog.Show(FragmentManager, null);
            };

            _manageTournamentButton.Enabled = false;
            _startEndTournamentButton.Enabled = false;
            _deleteTournamentButton.Enabled = false;

            if (!String.IsNullOrEmpty(TournamentId))
                _tournament = AppData.Service.RetrieveTournamentById(TournamentId);

            UpdateUI();

            return view;
        }
        
        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
            selectedDate = date;
            _tournamentDateEditText.Text = date.ToString("dd/MM/yyyy");
        }

        private void UpdateUI()
        {
            if (_tournament == null)
                return;

            _tournamentDateEditText.Text = _tournament.Date.ToString("dd/MM/yyyy");
            selectedDate = _tournament.Date;

            _createTournamentButton.Text = "Save Tournament";

            _manageTournamentButton.Enabled = true;
            _startEndTournamentButton.Enabled = true;
            
            _manageTournamentButton.Click += delegate
            {
                var manageTournamentIntent = new Intent(Activity, typeof(ManageTournamentActivity));
                manageTournamentIntent.PutExtra("tournamentId", _tournament.Id);
                StartActivity(manageTournamentIntent);
                //Activity.StartActivity(typeof(ManageTournamentActivity)); 
            };
            _deleteTournamentButton.Click += delegate { DeleteTournament(); };

            if (_tournament.Status == Domain.Enums.TournamentStatus.NotStarted)
            {
                _statusEditText.Text = "Not Started";
                _startEndTournamentButton.Text = "Start Tournament";
                _startEndTournamentButton.Click += delegate { StartTournament(); };
                _deleteTournamentButton.Enabled = true;
            }
            else if (_tournament.Status == Domain.Enums.TournamentStatus.InProgress)
            {
                _statusEditText.Text = "In Progress";
                _startEndTournamentButton.Text = "End Tournament";
                _startEndTournamentButton.Click += delegate { EndTournament(); };
                _deleteTournamentButton.Enabled = false;
            }
            else if (_tournament.Status == Domain.Enums.TournamentStatus.Completed)
            {
                _statusEditText.Text = "Completed";
                _createTournamentButton.Enabled = false;
                _startEndTournamentButton.Enabled = false;
                _deleteTournamentButton.Enabled = false;
            }
        }

        private void CreateTournament()
        {
            if (!errors)
            {
                string toastText = String.Empty;
                if (_tournament == null)
                {
                    _tournament = new Tournament();
                    toastText = "New tournament created.";
                    _tournament.Status = Domain.Enums.TournamentStatus.NotStarted;
                    _tournament.MaxPlayerPerTeam = 6;
                    _tournament.TotalTeam = 4;
                }
                else
                    toastText = "Tournament saved.";

                _tournament.Date = selectedDate;                                

                AppData.Service.AddEditTournament(_tournament);
                var toast = Toast.MakeText(Activity, toastText, ToastLength.Short);
                toast.Show();

                RefreshTournamentList();
                ClearDetailFragment();
            }
        }

        private void DeleteTournament()
        {
            AlertDialog.Builder alertConfirm = new AlertDialog.Builder(Activity);
            alertConfirm.SetCancelable(false);
            alertConfirm.SetPositiveButton("OK", ConfirmDelete);
            alertConfirm.SetNegativeButton("Cancel", delegate { });
            alertConfirm.SetMessage("Are you sure you want to delete this tournament?");
            alertConfirm.Show();
        }

        protected void ConfirmDelete(object sender, EventArgs e)
        {
            AppData.Service.DeleteTournament(_tournament.Id);

            var toast = Toast.MakeText(Activity, "Tournament successfully deleted", ToastLength.Short);
            toast.Show();

            RefreshTournamentList();
            ClearDetailFragment();
        }

        private void RefreshTournamentList()
        {
            AppData.Service.RefreshTournamentCache();

            var details = FragmentManager.FindFragmentById(Resource.Id.list_fragment) as PlayerTournamentFragment;

            if (details != null || details.tournamentAdapter != null)
            {
                // refresh tournament cache
                details.tournamentAdapter.NotifyDataSetChanged();
            }
        }

        private void ClearDetailFragment()
        {
            var details = FragmentManager.FindFragmentById(Resource.Id.fragmentContainer) as TournamentDetailFragment;

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
        
        private void StartTournament()
        {
            CreateTeams();
            AssignTeams();
            GenerateMatches();

            AppData.Service.StartTournament(_tournament);

            var toast = Toast.MakeText(Activity, String.Format("Tournament setup is successfully completed. The tournament shall start."), ToastLength.Short);
            toast.Show();

            _tournament = AppData.Service.RetrieveTournamentById(TournamentId);
            UpdateUI();
            RefreshTournamentList();
        }

        private void CreateTeams()
        {
            var teams = AppData.Service.GetAllTeams();
            
            var whiteTeam = teams.SingleOrDefault(t => t.Name.Equals("White", StringComparison.OrdinalIgnoreCase));

            if (whiteTeam == null)
            { 
                var wteam = new Team
                {
                    Name = "White"
                };

                AppData.Service.CreateTeam(wteam);
            }

            var blackTeam = teams.SingleOrDefault(t => t.Name.Equals("Black", StringComparison.OrdinalIgnoreCase));

            if (blackTeam == null)
            {
                var bteam = new Team
                {
                    Name = "Black"
                };

                AppData.Service.CreateTeam(bteam);
            }

            var redTeam = teams.SingleOrDefault(t => t.Name.Equals("Red", StringComparison.OrdinalIgnoreCase));

            if (redTeam == null)
            {
                var rteam = new Team
                {
                    Name = "Red"
                };

                AppData.Service.CreateTeam(rteam);
            }

            var blueTeam = teams.SingleOrDefault(t => t.Name.Equals("Blue", StringComparison.OrdinalIgnoreCase));

            if (blueTeam == null)
            {
                var blteam = new Team
                {
                    Name = "Blue"
                };

                AppData.Service.CreateTeam(blteam);
            }
            
        }

        private void AssignTeams()
        {
            var blueTeam = new Team("Blue");
            AppData.Service.AssignTeam(_tournament, blueTeam);

            var whiteTeam = new Team("White");
            AppData.Service.AssignTeam(_tournament, whiteTeam);

            var blackTeam = new Team("Black");
            AppData.Service.AssignTeam(_tournament, blackTeam);

            var redTeam = new Team("Red");
            AppData.Service.AssignTeam(_tournament, redTeam);
        }

        private void GenerateMatches()
        {
            AppData.Service.GenerateMatches(_tournament);
        }

        private void EndTournament()
        {
            try
            {
                AppData.Service.EndTournament(_tournament);
                var toast = Toast.MakeText(Activity, String.Format("Tournament successfully marked as ended."), ToastLength.Short);
                toast.Show();

                _tournament = AppData.Service.RetrieveTournamentById(TournamentId);
                UpdateUI();
                RefreshTournamentList();
            }
            catch (IncompleteMatchFoundException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        private void ShowErrorDialog(string error)
        {
            AlertDialog.Builder alertError = new AlertDialog.Builder(Activity);
            alertError.SetCancelable(false);
            alertError.SetPositiveButton("OK", delegate { });
            alertError.SetMessage(String.Format("Unable to end tournament. Error: {0}", error));
            alertError.Show();
        }
    }
}