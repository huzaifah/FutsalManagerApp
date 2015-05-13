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
using FutsalManager.Domain.Enums;

namespace FutsalManager.Droid.Adapters
{
    public class TournamentListViewAdapter : BaseAdapter<TournamentDto>
    {
        private readonly Activity _context;

        public TournamentListViewAdapter(Activity context)
        {
            _context = context;
        }

        public override TournamentDto this[int position]
        {
            get { return AppData.Service.Tournaments[position]; }
        }

        public override int Count
        {
            get { return AppData.Service.Tournaments.Count; }
        }

        public override long GetItemId(int position)
        {
            return AppData.Service.Tournaments[position].ListItemId;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
                view = _context.LayoutInflater.Inflate(Resource.Layout.TournamentListItem, null);

            var tournament = AppData.Service.Tournaments[position];

            if (String.IsNullOrEmpty(tournament.Id))
            {
                view.FindViewById<TextView>(Resource.Id.dateTextView).Text = "Add new tournament";
                view.FindViewById<TextView>(Resource.Id.statusTextView).Visibility = ViewStates.Invisible;
                view.FindViewById<TextView>(Resource.Id.championTextView).Visibility = ViewStates.Invisible;

                return view;
            }

            view.FindViewById<TextView>(Resource.Id.dateTextView).Text = tournament.Date.ToString("dd/MM/yyyy");
            view.FindViewById<TextView>(Resource.Id.championTextView).Text = "";

            var status = (TournamentStatus)Enum.Parse(typeof(TournamentStatus), tournament.Status);

            switch (status)
            {
                case TournamentStatus.NotStarted:
                    view.FindViewById<TextView>(Resource.Id.statusTextView).Text = "Not Started";
                    break;
                case TournamentStatus.InProgress:
                    view.FindViewById<TextView>(Resource.Id.statusTextView).Text = "In Progress";
                    break;
                case TournamentStatus.Completed:
                    view.FindViewById<TextView>(Resource.Id.statusTextView).Text = "Completed";
                    view.FindViewById<TextView>(Resource.Id.championTextView).Text = "Champion: ?";
                    break;
            }
                        
            view.FindViewById<TextView>(Resource.Id.statusTextView).Visibility = ViewStates.Visible;
            view.FindViewById<TextView>(Resource.Id.championTextView).Visibility = ViewStates.Visible;

            return view;
        }

    }
}