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
        public static Spell.Active Q = new Spell.Active(SpellSlot.Q);
        public static Spell.Active E = new Spell.Active(SpellSlot.E, 325);

        public static Spell.Skillshot R = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Cone, 250, 1600, 45)
        {
            AllowedCollisionCount = int.MaxValue
        };

        public static Menu Menu;

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
            get { return Menu["forcedRKeybind"].Cast<KeyBind>().CurrentValue; }
        }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Menu = MainMenu.AddMenu("Project:Riven", "projRiven");
            Menu.AddLabel("a simpler riven");
            Menu.AddLabel("VERY BETA, USE AT YOUR OWN RISK");
            Menu.Add("forcedRKeybind", new KeyBind("Forced R", false, KeyBind.BindTypes.PressToggle, 'T'));

            ItemHandler.Init();
            EventHandler.Init();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnTick += Game_OnTick;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var pos = Drawing.WorldToScreen(Player.Instance.Position);
            Text.Draw("Forced R: " + IsRActive, Color.AliceBlue, (int) pos.X - 45,
                (int) pos.Y + 40);
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