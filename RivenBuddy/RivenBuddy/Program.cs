using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using RivenBuddy.DamageIndicator;
using SharpDX;
using Color = SharpDX.Color;
using SpellData = RivenBuddy.DamageIndicator.SpellData;

namespace RivenBuddy
{
    class Program
    {
        
        public static Menu Menu, ComboMenu, HarassMenu, MinionClear, Jungle, DrawMenu;
        public static bool IsRActive { get { return ComboMenu["useR"].Cast<KeyBind>().CurrentValue; } }
        public static bool BurstActive { get { return ComboMenu["burst"].Cast<KeyBind>().CurrentValue; } }
        public static bool checkAA = false;
        public static bool FastQ;
        public static Text text = new Text("", new Font(FontFamily.GenericSansSerif, 9));
        public static DamageIndicator.DamageIndicator Indicator;
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Menu = MainMenu.AddMenu("RivenBuddy", "rivenbuddy");
            Menu.AddGroupLabel("Riven Buddy");
            Menu.AddSeparator();
            Menu.AddLabel("By Fluxy");
            Menu.AddLabel("nixi waz here");


            ComboMenu = Menu.AddSubMenu("Combo Settings", "combosettingsRiven");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("combo.useQ", new CheckBox("Use Q"));
            ComboMenu.Add("combo.useW", new CheckBox("Use W"));
            ComboMenu.Add("combo.useE", new CheckBox("Use E"));
            ComboMenu.Add("combo.useR", new CheckBox("Use R"));
            ComboMenu.Add("combo.useR2", new CheckBox("Use R2"));
            ComboMenu.Add("useR", new KeyBind("Force R", false, KeyBind.BindTypes.PressToggle, 'T'));
            ComboMenu.AddGroupLabel("Burst Combo");
            ComboMenu.Add("burst", new KeyBind("Burst", false, KeyBind.BindTypes.HoldActive, 'Y'));

            HarassMenu = Menu.AddSubMenu("Harass Settings", "harasssettingsRiven");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("harass.useQ", new CheckBox("Use Q"));
            HarassMenu.Add("harass.useW", new CheckBox("Use W"));
            HarassMenu.Add("harass.useE", new CheckBox("Use E"));

            MinionClear = Menu.AddSubMenu("Minion Clear Settings", "farmettingsRiven");
            MinionClear.AddGroupLabel("LastHit Settings");
            MinionClear.Add("lasthit.useQ", new CheckBox("Use Q"));
            MinionClear.Add("lasthit.useW", new CheckBox("Use W"));
            MinionClear.AddSeparator();
            MinionClear.AddGroupLabel("Wave Clear Settings");
            MinionClear.Add("waveclear.useQ", new CheckBox("Use Q"));
            MinionClear.Add("waveclear.useW", new CheckBox("Use W"));

            Jungle = Menu.AddSubMenu("Jungle Settings", "jungleettingsRiven");
            Jungle.AddGroupLabel("Jungle Clear Settings");
            Jungle.Add("jungle.useQ", new CheckBox("Use Q"));
            Jungle.Add("jungle.useW", new CheckBox("Use W"));
            Jungle.Add("jungle.useE", new CheckBox("Use E"));

            DrawMenu = Menu.AddSubMenu("Draw Settings", "drawsettingsRiven");
            DrawMenu.AddGroupLabel("Draw Settings");
            DrawMenu.Add("draw.Q", new CheckBox("Draw Q", false));
            DrawMenu.Add("draw.W", new CheckBox("Draw W", false));
            DrawMenu.Add("draw.E", new CheckBox("Draw E", false));
            DrawMenu.Add("draw.R", new CheckBox("Draw R", false));
            DrawMenu.Add("draw.Combo", new CheckBox("Write Current Combo", false));
            DrawMenu.Add("draw.rState", new CheckBox("Write R State"));

            Indicator = new DamageIndicator.DamageIndicator();
            Indicator.Add("Combo", new SpellData(0, DamageType.True, System.Drawing.Color.Aqua));

            TargetSelector2.Init();
            SpellEvents.Init();
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += delegate { SpellManager.UpdateSpells(); };

            Chat.Print("RivenBuddy : Fully Loaded. by fluxy");
        }
        

        private static void Drawing_OnDraw(EventArgs args)
        {
            if(DrawMenu["draw.rState"].Cast<CheckBox>().CurrentValue) text.Draw("Forced R: " + IsRActive, System.Drawing.Color.AliceBlue, (int) Player.Instance.HPBarPosition.X - 8, (int) Player.Instance.HPBarPosition.Y);
            if (DrawMenu["draw.Combo"].Cast<CheckBox>().CurrentValue)
            {
                var s = Queuer.Queue.Aggregate("", (current, VARIABLE) => current + (" " + VARIABLE));
                Drawing.DrawText(100, 100, System.Drawing.Color.Wheat, s);
            }
            if (DrawMenu["draw.Q"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.Spells[SpellSlot.Q].IsReady() ? Color.Cyan : Color.OrangeRed, SpellManager.Spells[SpellSlot.Q].Range, Player.Instance.Position);
            }
            if (DrawMenu["draw.W"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.Spells[SpellSlot.W].IsReady() ? Color.Cyan : Color.OrangeRed, SpellManager.Spells[SpellSlot.W].Range, Player.Instance.Position);
            }
            if (DrawMenu["draw.E"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.Spells[SpellSlot.E].IsReady() ? Color.Cyan : Color.OrangeRed, SpellManager.Spells[SpellSlot.E].Range, Player.Instance.Position);
            }
            if (DrawMenu["draw.R"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.Spells[SpellSlot.R].IsReady() ? Color.Cyan : Color.OrangeRed, SpellManager.Spells[SpellSlot.R].Range, Player.Instance.Position);
            }

        }
        
        private static void Game_OnUpdate(EventArgs args)
        {
            Orbwalker.ForcedTarget = null;
            Queuer.tiamat =
                ObjectManager.Player.InventoryItems.FirstOrDefault(
                    a => a.Id == ItemId.Tiamat_Melee_Only || a.Id == ItemId.Ravenous_Hydra_Melee_Only);
            Indicator.Update("Combo", new SpellData((int) DamageHandler.ComboDamage(), DamageType.Physical, System.Drawing.Color.Aqua));
            
            if (BurstActive)
            {
                States.Burst();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                States.Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                States.Harass();
            }
            if(Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                States.Jungle();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                var target = EntityManager.GetJungleMonsters(Player.Instance.Position.To2D(),
                   SpellManager.Spells[SpellSlot.Q].Range + 300).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) && target == null)
                {
                    States.WaveClear();
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                States.LastHit();
            }
        }
    }
}
