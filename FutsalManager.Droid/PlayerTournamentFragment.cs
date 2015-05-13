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
using FutsalManager.Droid.Adapters;

namespace FutsalManager.Droid
{
    public class PlayerTournamentFragment : ListFragment
    {
        public PlayerListViewAdapter playerAdapter;
        public TournamentListViewAdapter tournamentAdapter;

        public static PlayerTournamentFragment NewInstance(bool isPlayer)
        {
            var playerTournamentFragment = new PlayerTournamentFragment { Arguments = new Bundle() };
            playerTournamentFragment.Arguments.PutBoolean("is_player", isPlayer);

            return playerTournamentFragment;
        }

        public bool IsPlayer
        {
            get
            {
                if (Arguments == null)
                    return true;
                else
                    return Arguments.GetBoolean("is_player", true);
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //// Create your fragment here
            //isPlayer = ((MasterDataActivity)Activity).IsPlayer;

            if (IsPlayer)
            {
                // load player list
                playerAdapter = new PlayerListViewAdapter(Activity);
                ListAdapter = playerAdapter;

                AppData.Service.RefreshPlayerCache();
                playerAdapter.NotifyDataSetChanged();
            }
            else
            {
                // load tournament list
                tournamentAdapter = new TournamentListViewAdapter(Activity);
                ListAdapter = tournamentAdapter;

                AppData.Service.RefreshTournamentCache();
                tournamentAdapter.NotifyDataSetChanged();
            }

            ClearFragment();
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            if (IsPlayer)
                ShowPlayerDetails(position, id);
            else
                ShowTournamentDetails(position, id);
        }

        private void ShowPlayerDetails(int position, long id)
        {
            ListView.SetItemChecked(position, true);

            var details = FragmentManager.FindFragmentById(Resource.Id.fragmentContainer) as PlayerDetailFragment;
            var player = AppData.Service.GetPlayerByItemId(id);

            if (details == null || details.PlayerId != player.Id)
            {
                details = PlayerDetailFragment.NewInstance(player.Id);

                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragmentContainer, details);
                ft.SetTransition(FragmentTransit.FragmentFade);
                ft.Commit();
            }
        }

        private void ShowTournamentDetails(int position, long id)
        {
            ListView.SetItemChecked(position, true);

            var details = FragmentManager.FindFragmentById(Resource.Id.fragmentContainer) as TournamentDetailFragment;
            var tournament = AppData.Service.GetTournamentByItemId(id);

            if (details == null || details.TournamentId != tournament.Id)
            {
                details = TournamentDetailFragment.NewInstance(tournament.Id);

                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragmentContainer, details);
                ft.SetTransition(FragmentTransit.FragmentFade);
                ft.Commit();
            }
        }

        private void ClearFragment()
        {
            Fragment details;
            
            if (!IsPlayer)
                details = FragmentManager.FindFragmentById(Resource.Id.fragmentContainer) as PlayerDetailFragment;             
            else
                details = FragmentManager.FindFragmentById(Resource.Id.fragmentContainer) as TournamentDetailFragment;             

            if (details == null)
                return;

            var ft = FragmentManager.BeginTransaction();
            ft.Remove(details);
            ft.SetTransition(FragmentTransit.FragmentFade);
            ft.Commit();
        }
    }
}