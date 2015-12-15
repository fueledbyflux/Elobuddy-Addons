using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;

namespace ProjectRiven
{
    internal class Riven
    {
        public static Text Text = new Text("", new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold));
        public static Spell.Active Q = new Spell.Active(SpellSlot.Q, 300);
        public static Spell.Active E = new Spell.Active(SpellSlot.E, 325);

        public static Spell.Skillshot R = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Cone, 250, 1600, 45)
        {
            AllowedCollisionCount = int.MaxValue
        };

        public static Menu Menu, ComboMenu, HarassMenu, FarmMenu, MiscMenu;

        public static Spell.Active W
        {
            get
            {
                return new Spell.Active(SpellSlot.W,
                    (uint)
                        (70 + Player.Instance.BoundingRadius +
                         (Player.Instance.HasBuff("RivenFengShuiEngine") ? 195 : 120)));
            }
        }

        public static bool IsRActive
        {
            get
            {
                return ComboMenu["forcedRKeybind"].Cast<KeyBind>().CurrentValue &&
                       ComboMenu["Combo.R"].Cast<CheckBox>().CurrentValue;
            }
        }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Riven) return;
            Menu = MainMenu.AddMenu("Project:Riven", "projRiven");
            Menu.AddLabel("a simpler riven");

            ComboMenu = Menu.AddSubMenu("Combo Settings", "comboSettings");
            ComboMenu.Add("Combo.Q", new CheckBox("Use Q"));
            ComboMenu.Add("Combo.W", new CheckBox("Use W"));
            ComboMenu.Add("Combo.E", new CheckBox("Use E"));
            ComboMenu.Add("Combo.R2", new CheckBox("Use R Wave (Killable)"));
            ComboMenu.AddLabel("R1 Settings");
            ComboMenu.Add("Combo.R", new CheckBox("Use R"));
            ComboMenu.Add("forcedRKeybind", new KeyBind("Forced R", false, KeyBind.BindTypes.PressToggle, 'T'));
            ComboMenu.AddLabel("When To use R");
            ComboMenu.Add("Combo.RCombo", new CheckBox("Cant Kill with Combo"));
            ComboMenu.Add("Combo.RPeople", new CheckBox("Have more than 1 person near"));

            HarassMenu = Menu.AddSubMenu("Harass Settings", "harassSettings");
            HarassMenu.Add("Harass.Q", new CheckBox("Use Q"));
            HarassMenu.Add("Harass.W", new CheckBox("Use W"));
            HarassMenu.Add("Harass.E", new CheckBox("Use E"));

            FarmMenu = Menu.AddSubMenu("Farm Settings", "farmSettings");
            FarmMenu.AddLabel("Last Hit");
            FarmMenu.Add("LastHit.Q", new CheckBox("Use Q"));
            FarmMenu.Add("LastHit.W", new CheckBox("Use W"));
            FarmMenu.AddLabel("Wave Clear");
            FarmMenu.Add("WaveClear.Q", new CheckBox("Use Q"));
            FarmMenu.Add("WaveClear.W", new CheckBox("Use W"));
            FarmMenu.AddLabel("Jungle");
            FarmMenu.Add("Jungle.Q", new CheckBox("Use Q"));
            FarmMenu.Add("Jungle.W", new CheckBox("Use W"));
            FarmMenu.Add("Jungle.E", new CheckBox("Use E"));

            MiscMenu = Menu.AddSubMenu("Misc Settings", "miscSettings");
            MiscMenu.AddLabel("Keep Alive Settings");
            MiscMenu.Add("Alive.Q", new CheckBox("Keep Q Alive"));
            MiscMenu.Add("Alive.R", new CheckBox("Use R2 Before Expire"));
            MiscMenu.AddLabel("Humanizer Settings");
            MiscMenu.Add("HumanizerDelay", new Slider("Humanizer Delay (ms)", 0, 0, 300));


            ItemHandler.Init();
            EventHandler.Init();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnTick += Game_OnTick;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (ComboMenu["Combo.R"].Cast<CheckBox>().CurrentValue)
            {
                var pos = Drawing.WorldToScreen(Player.Instance.Position);
                Text.Draw("Forced R: " + IsRActive, Color.AliceBlue, (int) pos.X - 45,
                    (int) pos.Y + 40);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                StateHandler.Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                StateHandler.Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                StateHandler.LastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                StateHandler.LaneClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                StateHandler.Jungle();
            }
        }
    }
}