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
    public class TeamAssignmentListFragment : ListFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            var adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItemChecked, AppData.Service.GetAllTeams().OrderBy(p => p.Name).Select(t => t.Name).ToArray());
            ListAdapter = adapter;
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            //base.OnListItemClick(l, v, position, id);
        }
    }
}