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

namespace FutsalManager.Droid
{
    public class MatchListFragment : ListFragment
    {
        Tournament _tournament;

        public static MatchListFragment NewInstance(string tournamentId)
        {
            var tournamentDetailFragment = new MatchListFragment { Arguments = new Bundle() };
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

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (!String.IsNullOrEmpty(TournamentId))
            { 
                _tournament = AppData.Service.RetrieveTournamentById(TournamentId);

                var matches = AppData.Service.RetrieveMatches(_tournament).ToList().ConvertAll(match => match.HomeTeam.Name + " vs " + match.AwayTeam.Name).ToArray();
                
                // Create your fragment here
                var adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItemChecked, matches);
                ListAdapter = adapter;
            }
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            //base.OnListItemClick(l, v, position, id);
        }
    }
}