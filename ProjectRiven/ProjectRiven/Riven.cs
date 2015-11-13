using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;

namespace ProjectRiven
{
    class Riven
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static Spell.Active Q = new Spell.Active(SpellSlot.Q);
        public static Spell.Active E = new Spell.Active(SpellSlot.E);

        public static Spell.Active W
        {
            get
            {
                return new Spell.Active(SpellSlot.W, (uint) (70 + Player.Instance.BoundingRadius + (Player.Instance.HasBuff("RivenFengShuiEngine") ? 195 : 120)));
            }
        }

        public static Spell.Skillshot R = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Cone, 250, 1600, 45) {AllowedCollisionCount = int.MaxValue};

        public static Menu Menu;

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Menu = MainMenu.AddMenu("Project:Riven", "projRiven");
            Menu.AddLabel("a simpler riven");
            Menu.AddLabel("VERY BETA, USE AT YOUR OWN RISK");

            ItemHandler.Init();
            EventHandler.Init();
        }
    }
}
