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
using FutsalManager.Domain.Dtos;

namespace FutsalManager.Droid.Adapters
{
    public class PlayerListViewAdapter : BaseAdapter<PlayerDto>
    {
        private readonly Activity _context;

        public PlayerListViewAdapter(Activity context)
        {
            _context = context;
        }

        public override PlayerDto this[int position]
        {
            get { return AppData.Service.Players[position]; }
        }

        public override int Count
        {
            get { return AppData.Service.Players.Count; }
        }

        public override long GetItemId(int position)
        {
            return AppData.Service.Players[position].ListItemId;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
                view = _context.LayoutInflater.Inflate(Resource.Layout.PlayerListItem, null);

            var player = AppData.Service.Players[position];

            string age = "??";

            if (player.Name == "0")
            {
                view.FindViewById<TextView>(Resource.Id.nameTextView).Text = "Add new player";
                view.FindViewById<TextView>(Resource.Id.positionTextView).Visibility = ViewStates.Invisible;
                view.FindViewById<TextView>(Resource.Id.goalTextView).Visibility = ViewStates.Invisible;
                return view;
            }

            if (player.Age != 0)
                age = player.Age.ToString();

            view.FindViewById<TextView>(Resource.Id.nameTextView).Text = player.Name + " (" + age + ")";
            view.FindViewById<TextView>(Resource.Id.positionTextView).Text = player.Position;
            view.FindViewById<TextView>(Resource.Id.goalTextView).Text = "Goals: 0";
            view.FindViewById<TextView>(Resource.Id.positionTextView).Visibility = ViewStates.Visible;
            view.FindViewById<TextView>(Resource.Id.goalTextView).Visibility = ViewStates.Visible;

            return view;
        }
    }
}