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

namespace FutsalManager.Droid
{
    [Activity(Label = "Master Data")]
    public class MasterDataActivity : Activity
    {
        //public bool IsPlayer { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.MasterData);

        }
    }
}