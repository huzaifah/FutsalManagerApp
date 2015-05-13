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
    public class MasterDataFragment : ListFragment
    {
        //public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    var view = inflater.Inflate(Resource.Layout.MasterDataMenu, null);
        //    return view;
        //}

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            var adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItemSingleChoice, MenuArrays.MasterData);
            ListAdapter = adapter;
            //ListView.ChoiceMode = ChoiceMode.Single;
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            ShowFragment(position);
        }

        private void ShowFragment(int position)
        {
            ListView.SetItemChecked(position, true);

            // check what fragment is shown
            var details = FragmentManager.FindFragmentById(Resource.Id.list_fragment) as PlayerTournamentFragment;
            var isPlayer = position == 0 ? true : false;

            if (details == null || details.IsPlayer != isPlayer)
            {
                // make new fragment to show this selection
                details = PlayerTournamentFragment.NewInstance(isPlayer);

                // execute a transaction, replacing any existing fragment with this one inside the frame
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.list_fragment, details);
                ft.SetTransition(FragmentTransit.FragmentFade);
                ft.Commit();
            }
        }
    }
}