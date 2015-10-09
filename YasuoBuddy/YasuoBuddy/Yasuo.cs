using System;
using System.Drawing;
using System.Security.AccessControl;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;

namespace YasuoBuddy
{
    internal class Yasuo
    {
        public static Menu Menu, ComboMenu, HarassMenu, FarmMenu, FleeMenu, DrawMenu;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Menu = MainMenu.AddMenu("YasuoBuddy", "yasuobuddyfluxy");

            ComboMenu = Menu.AddSubMenu("Combo", "yasuCombo");
            ComboMenu.Add("combo.Q", new CheckBox("Use Q"));
            ComboMenu.Add("combo.E", new CheckBox("Use E"));
            ComboMenu.Add("combo.stack", new CheckBox("Stack Q"));
            ComboMenu.AddSeparator();
            ComboMenu.Add("combo.R", new CheckBox("Use R"));
            ComboMenu.Add("combo.RTarget", new CheckBox("Use R always on Selected Target"));
            ComboMenu.Add("combo.RKillable", new CheckBox("Use R Execute"));
            ComboMenu.Add("combo.MinTargetsR", new Slider("Use R Min Targets", 2, 1, 5));

            HarassMenu = Menu.AddSubMenu("Harass", "yasuHarass");
            HarassMenu.Add("harass.Q", new CheckBox("Use Q"));
            HarassMenu.Add("harass.E", new CheckBox("Use E"));
            HarassMenu.Add("harass.stack", new CheckBox("Stack Q"));

            FarmMenu = Menu.AddSubMenu("Farming Settings", "yasuoFarm");
            FarmMenu.AddGroupLabel("Last Hit");
            FarmMenu.Add("LH.Q", new CheckBox("Use Q"));
            FarmMenu.Add("LH.E", new CheckBox("Use E"));

            FarmMenu.AddGroupLabel("WaveClear");
            FarmMenu.Add("WC.Q", new CheckBox("Use Q"));
            FarmMenu.Add("WC.E", new CheckBox("Use E"));

            FarmMenu.AddGroupLabel("Jungle");
            FarmMenu.Add("JNG.Q", new CheckBox("Use Q"));
            FarmMenu.Add("JNG.E", new CheckBox("Use E"));

            FleeMenu = Menu.AddSubMenu("Flee", "yasuoFlee");
            FleeMenu.Add("Flee.E", new CheckBox("Use E"));
            FleeMenu.Add("Flee.stack", new CheckBox("Stack Q"));

            DrawMenu = Menu.AddSubMenu("Draw", "yasuoDraw");
            DrawMenu.Add("Draw.Q", new CheckBox("Draw Q", false));
            DrawMenu.Add("Draw.E", new CheckBox("Draw E", false));
            DrawMenu.Add("Draw.R", new CheckBox("Draw R", false));


            EvadePlus.Program.Main(null);
            TargetedSpells.SpellDetectorWindwaller.Init();
            TargetSelector2.Init();
            EventManager.Init();
            Game.OnTick += Game_OnTick;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Drawing.OnDraw += Drawing_OnDraw;
            Chat.Print("Yasuo Loaded", Color.AliceBlue);
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsValid && sender.Team == ObjectManager.Player.Team && args.SData.Name == "YasuoWMovingWall")
            {
                EEvader.YasuoWallCastedPos = sender.ServerPosition.To2D();
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu["Draw.Q"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.Q().IsReady() ? SharpDX.Color.Cyan : SharpDX.Color.DarkRed,
                    SpellManager.Q().Range, Player.Instance.Position);
            }
            if (DrawMenu["Draw.R"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.R.IsReady() ? SharpDX.Color.Cyan : SharpDX.Color.DarkRed,
                    SpellManager.R.Range, Player.Instance.Position);
            }
            if (DrawMenu["Draw.E"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.E.IsReady() ? SharpDX.Color.Cyan : SharpDX.Color.DarkRed,
                    SpellManager.E.Range, Player.Instance.Position);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            EEvader.UpdateTask();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                StateManager.Flee();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                StateManager.Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                StateManager.LastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                StateManager.WaveClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                StateManager.Jungle();
            }
        }
    }
}