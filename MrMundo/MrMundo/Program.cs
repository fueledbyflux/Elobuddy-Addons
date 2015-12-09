using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using MrMundo.States;

namespace MrMundo
{
    class Program
    {
        public static Menu Menu, ComboMenu, HarassMenu, FarmMenu;
        public static int ResetTime;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.DrMundo) return;

            Menu = MainMenu.AddMenu("MrMundo", "mrmundo");

            ComboMenu = Menu.AddSubMenu("Combo", "mrmundoCombo", "Mr Mundo - Combo");
            ComboMenu.Add("useQCombo", new CheckBox("Use Q"));
            ComboMenu.Add("useWCombo", new CheckBox("Use W"));
            ComboMenu.Add("useECombo", new CheckBox("Use E"));
            ComboMenu.Add("useRCombo", new CheckBox("Use R"));
            ComboMenu.Add("useRComboHPPercent", new Slider("HP %", 30));
            ComboMenu.Add("useRComboEnemies", new Slider("Min Enemies", 0, 0, 5));

            HarassMenu = Menu.AddSubMenu("Harass", "mrmundoHarass", "Mr Mundo - Harass");
            HarassMenu.Add("useQHarass", new CheckBox("Use Q"));
            HarassMenu.Add("useWHarass", new CheckBox("Use W"));
            HarassMenu.Add("useEHarass", new CheckBox("Use E"));

            FarmMenu = Menu.AddSubMenu("Farming", "mrmundoFarming", "Mr Mundo - Farming");
            FarmMenu.AddLabel("Last Hit");
            FarmMenu.Add("useQLH", new CheckBox("Use Q"));
            FarmMenu.AddLabel("Wave Clear");
            FarmMenu.Add("useQWC", new CheckBox("Use Q"));
            FarmMenu.AddLabel("Jungle");
            FarmMenu.Add("useQJNG", new CheckBox("Use Q"));

            StateHandler.Init();
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        public static bool HasW { get { return Player.HasBuff("BurningAgony") || ResetTime > Environment.TickCount; } }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || args.SData.Name != Player.GetSpell(SpellSlot.W).Name) return;
            if (!Player.HasBuff("BurningAgony"))
            {
                ResetTime = Environment.TickCount + 500;
            }
        }
    }
}
