using System;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Tracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }

        public static Menu TrackerMenu;

        private static void Game_OnStart(EventArgs args)
        {
            TrackerMenu = MainMenu.AddMenu("Tracker", "trackerMenu");
            TrackerMenu.AddGroupLabel("Tracker");
            TrackerMenu.AddSeparator();
            TrackerMenu.Add("showAllies", new CheckBox("Show Allies"));
            TrackerMenu.Add("showEnemies", new CheckBox("Show Enemies"));
            TrackerMenu.Add("showTimer", new CheckBox("Show Cooldown Timer"));
            TrackerMenu.Add("showXp", new CheckBox("Show Experience"));
            TrackerMenu.AddSeparator();
            TrackerMenu.AddLabel("By: The Elobuddy Team");

            OfficialAddon.Initialize();
        }
    }
}
