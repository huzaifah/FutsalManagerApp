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

namespace FutsalManager.Droid
{
    public class ManageTournamentFragment : ListFragment
    {
        private string _tournamentId;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            var adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItemSingleChoice, MenuArrays.ManageTournament);
            ListAdapter = adapter;
            //ListView.ChoiceMode = ChoiceMode.Single;

            _tournamentId = (Activity as ManageTournamentActivity).TournamentId;
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            ShowFragment(position);
        }

        private void ShowFragment(int position)
        {
            ListView.SetItemChecked(position, true);

            Fragment details;

            switch (position)
            {
                case 0: // team assignment
                    details = FragmentManager.FindFragmentById(Resource.Id.list_fragment) as TeamAssignmentListFragment;

                    if (details == null)
                    {
                        // make new fragment to show this selection
                        details = new TeamAssignmentListFragment();

                        // execute a transaction, replacing any existing fragment with this one inside the frame
                        var ft = FragmentManager.BeginTransaction();
                        ft.Replace(Resource.Id.list_fragment, details);
                        ft.SetTransition(FragmentTransit.FragmentFade);
                        ft.Commit();
                    }

                    break;

                case 2: // team assignment
                    details = FragmentManager.FindFragmentById(Resource.Id.list_fragment) as MatchListFragment;

                    if (details == null)
                    {
                        // make new fragment to show this selection

                        details = MatchListFragment.NewInstance(_tournamentId);

                        // execute a transaction, replacing any existing fragment with this one inside the frame
                        var ft = FragmentManager.BeginTransaction();
                        ft.Replace(Resource.Id.list_fragment, details);
                        ft.SetTransition(FragmentTransit.FragmentFade);
                        ft.Commit();
                    }

                    break;
                default:
                    break;
            }

            //// check what fragment is shown
            //var details = FragmentManager.FindFragmentById(Resource.Id.list_fragment) as PlayerTournamentFragment;
            //var isPlayer = position == 0 ? true : false;

            //if (details == null || details.IsPlayer != isPlayer)
            //{
            //    // make new fragment to show this selection
            //    details = PlayerTournamentFragment.NewInstance(isPlayer);

            //    // execute a transaction, replacing any existing fragment with this one inside the frame
            //    var ft = FragmentManager.BeginTransaction();
            //    ft.Replace(Resource.Id.list_fragment, details);
            //    ft.SetTransition(FragmentTransit.FragmentFade);
            //    ft.Commit();
            //}
        }
    }
}