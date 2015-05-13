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
    public class DatePickerDialogFragment : DialogFragment
    {
        private readonly Context _context;
        private DateTime _date;
        private readonly Android.App.DatePickerDialog.IOnDateSetListener _listener;

        public DatePickerDialogFragment(Context context, DateTime date, Android.App.DatePickerDialog.IOnDateSetListener listener)
        {
            _context = context;
            _date = date;
            _listener = listener;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = new Android.App.DatePickerDialog(_context, _listener, _date.Year, _date.Month - 1, _date.Day);
            return dialog;
        }
    }
}