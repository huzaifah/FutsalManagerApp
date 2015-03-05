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
using System.IO;
using FutsalManager.Domain;
using FutsalManager.Persistence;

namespace FutsalManager.Droid.Adapters
{
    public class AppData
    {
        public static readonly TournamentService Service = new TournamentService(new TournamentRepository(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path,
                "Bobai", "futsal.db")));
    }
}