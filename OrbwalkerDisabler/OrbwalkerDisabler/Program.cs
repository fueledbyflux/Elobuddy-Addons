using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace OrbwalkerDisabler
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static Menu Menu;

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Menu = MainMenu.AddMenu("Orbwalker Disabler", "orbws");
            var a = Menu.Add("disableMovement", new CheckBox("Disable Movement", false));
            var b = Menu.Add("disableAttacks", new CheckBox("Disable Movement", false));

            Game.OnTick += delegate
            {
                Orbwalker.DisableAttacking = b.CurrentValue;
                Orbwalker.DisableMovement = a.CurrentValue;
            };
        }
    }
}
