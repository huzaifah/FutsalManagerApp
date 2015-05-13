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
    internal class MenuArrays
    {
        internal static string[] MasterData =
        {
            "Players",
            "Tournaments"
        };

        internal static string[] ManageTournament =
        {
            "Team Assignment",
            "Player Status",
            "Matches",
            "Team Standings",
            "Tournament Stats"
        };

        internal static string[] Teams =
        {
            "Black",
            "Blue",
            "Red",
            "White"
        };
    }
}