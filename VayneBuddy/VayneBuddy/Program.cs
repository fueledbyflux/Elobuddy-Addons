using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using VayneBuddy.Activator;
using Color = System.Drawing.Color;

namespace VayneBuddy
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static Spell.Ranged Q;
        public static Spell.Targeted E;
        public static Spell.Active R;

        public static Menu Menu,
            ComboMenu,
            HarassMenu,
            FarmMenu,
            CondemnMenu,
            DrawMenu,
            InterruptorMenu,
            GapCloserMenu,
            CondemnPriorityMenu;
        
        public static string[] DangerSliderValues = {"Low", "Medium", "High"};
        public static string[] PriorityValues = {"Very Low", "Low", "Medium", "High", "Very High"};
        public static List<Vector2> Points = new List<Vector2>();
        public static List<Vector2> CorrectPoints = new List<Vector2>();

        private static void Game_OnStart(EventArgs args)
        {
            if (!_Player.ChampionName.ToLower().Contains("vayne")) return;

            Bootstrap.Init(null);
            ItemManager.Init();
            TargetSelector2.init();

            Q = new Spell.Skillshot(SpellSlot.Q, (uint) _Player.GetAutoAttackRange(), SkillShotType.Circular);
            E = new Spell.Targeted(SpellSlot.E, (uint) _Player.GetAutoAttackRange());
            R = new Spell.Active(SpellSlot.R);
            

            Menu = MainMenu.AddMenu("Vayne Buddy", "vBuddy");

            Menu.AddGroupLabel("Vayne Buddy");
            Menu.AddLabel("Version: " + "0.0.0.1");
            Menu.AddSeparator();
            Menu.AddLabel("By Fluxy ;)");
            Menu.AddSeparator();
            Menu.AddLabel("p.s. imeh is faggot");

            ComboMenu = Menu.AddSubMenu("Combo", "vBuddyCombo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("useQCombo", new CheckBox("Use Q", true));
            ComboMenu.AddLabel("R Settings");
            ComboMenu.Add("useRCombo", new CheckBox("Use R"));
            ComboMenu.Add("noRUnderTurret", new CheckBox("Disable R if Target is under allied turret", true));

            CondemnPriorityMenu = Menu.AddSubMenu("Auto Condemn", "vBuddyCondemnPriority");
            CondemnPriorityMenu.AddGroupLabel("Condemn Priority");
            foreach (var enem in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy))
            {
                var champValue = CondemnPriorityMenu.Add(enem.ChampionName + "priority", new Slider(enem.ChampionName + ": ", 1, 1, 5));
                var enem1 = enem;
                champValue.OnValueChange += delegate
                {
                    champValue.DisplayName = enem1.ChampionName + ": " + PriorityValues[champValue.CurrentValue];
                };
                champValue.DisplayName = enem1.ChampionName + ": " + PriorityValues[champValue.CurrentValue];
            }
            CondemnPriorityMenu.AddSeparator();
            var sliderValue = CondemnPriorityMenu.Add("minSliderAutoCondemn", new Slider("Min Priority for Auto Condemn: ", 2, 1, 5));
            sliderValue.OnValueChange += delegate
            {
                sliderValue.DisplayName = "Min Priority for Auto Condemn: " + PriorityValues[sliderValue.CurrentValue];
            };
            sliderValue.DisplayName = "Min Priority for Auto Condemn: " + PriorityValues[sliderValue.CurrentValue];
            CondemnPriorityMenu.Add("autoCondemnToggle",
                new KeyBind("Auto Condemn", false, KeyBind.BindTypes.PressToggle, 'H'));
            CondemnPriorityMenu.AddSeparator();

            CondemnMenu = Menu.AddSubMenu("Condemn", "vBuddyCondemn");
            CondemnMenu.AddGroupLabel("Condemn Settings");
            CondemnMenu.AddSeparator();
            CondemnMenu.Add("pushDistance", new Slider("Push Distance", 410, 350, 420));
            CondemnMenu.AddSeparator();
            CondemnMenu.AddLabel("Active Mode Settings");
            CondemnMenu.Add("smartVsCheap",
                new CheckBox("On (SMART CONDEMN (saves fps)) / OFF (360 degree check)", true));
            CondemnMenu.AddSeparator();
            CondemnMenu.Add("condemnCombo", new CheckBox("Condemn in Combo", true));
            CondemnMenu.Add("condemnComboTrinket", new CheckBox("Trinket Bush After E", true));
            CondemnMenu.Add("condemnHarass", new CheckBox("Condemn in Harass", true));

            HarassMenu = Menu.AddSubMenu("Harass", "vBuddyHarass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("useQHarass", new CheckBox("Use Q", true));

            FarmMenu = Menu.AddSubMenu("Farming", "vBuddyFarm");
            FarmMenu.AddGroupLabel("Farming Settings");
            FarmMenu.Add("customLastHitWaveClearMode", new CheckBox("Custom LH / WC"));
            FarmMenu.Add("MinManaQLHWC", new Slider("Minimum Mana % for Farm", 30));
            FarmMenu.AddLabel("Last Hit");
            FarmMenu.Add("useQLastHit", new CheckBox("Use Q Last", true));
            FarmMenu.AddLabel("WaveClear");
            FarmMenu.Add("useQWaveClear", new CheckBox("Use Q WaveClear", true));

            DrawMenu = Menu.AddSubMenu("Misc Menu", "vBuddyMisc");
            DrawMenu.AddGroupLabel("Draw Settings");
            DrawMenu.Add("drawERange", new CheckBox("Draw E Range"));
            DrawMenu.Add("condemnVisualiser", new CheckBox("Draw Condemn"));
            DrawMenu.Add("drawStacks", new CheckBox("Draw W Stacks"));
            DrawMenu.AddLabel("Misc");
            DrawMenu.Add("wallJumpKey", new KeyBind("Tumble Walls", false, KeyBind.BindTypes.HoldActive, 'Z'));

            InterruptorMenu = Menu.AddSubMenu("Interrupter", "InterruptorvBuddy");
            InterruptorMenu.AddGroupLabel("Interrupter Menu");
            InterruptorMenu.Add("enableInterrupter", new CheckBox("Enable Interrupter"));
            InterruptorMenu.AddSeparator();
            var dangerSlider = InterruptorMenu.Add("dangerLevel", new Slider("Set Your Danger Level: ", 3, 1, 3));
            var dangerSliderDisplay = InterruptorMenu.Add("dangerLevelDisplay",
                new Label("Danger Level: " + DangerSliderValues[dangerSlider.Cast<Slider>().CurrentValue - 1]));
            dangerSlider.Cast<Slider>().OnValueChange += delegate
            {
                dangerSliderDisplay.Cast<Label>().DisplayName =
                    "Danger Level: " + DangerSliderValues[dangerSlider.Cast<Slider>().CurrentValue - 1];
            };

            GapCloserMenu = Menu.AddSubMenu("Anti-GapClosers", "gapClosersvBuddy");
            GapCloserMenu.AddGroupLabel("Anti-GapCloser Menu");
            GapCloserMenu.Add("enableGapCloser", new CheckBox("Enable Anti-GapCloser"));


            Orbwalker.OnPostAttack += Events.Orbwalker_OnPostAttack;
            Orbwalker.OnPreAttack += Events.Orbwalker_OnPreAttack;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Gapcloser.OnGapCloser += Events.Gapcloser_OnGapCloser;
            Interrupter.OnInterruptableSpell += Events.Interrupter_OnInterruptableSpell;
            AIHeroClient.OnProcessSpellCast += AIHeroClient_OnProcessSpellCast;

            Chat.Print("VayneBuddy: Loading Tasks Done.", Color.Firebrick);
            Chat.Print("VayneBuddy: iMeh is faggot.", Color.Firebrick);

        }

        private static void AIHeroClient_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;
            if (args.SData.Name.ToLower().Contains("vaynetumble"))
            {
                Core.DelayAction(Orbwalker.ResetAutoAttack, 250);
            }
            if (args.SData.Name.ToLower().Equals("vaynecondemn") && args.Target != null)
            {
                Events.LastAa = Environment.TickCount;
                if (_Player.Spellbook.GetSpell(SpellSlot.W).IsLearned)
                {
                    if (Events.AAedTarget != null && args.Target.NetworkId == Events.AAedTarget.NetworkId)
                    {
                        switch (Events.AaStacks)
                        {
                            case 0:
                                Events.AaStacks = 1;
                                break;
                            case 1:
                                Events.AaStacks = 2;
                                break;
                            case 2:
                                Events.AaStacks = 0;
                                Events.AAedTarget = null;
                                break;
                        }
                    }
                    else
                    {
                        Events.AAedTarget = args.Target as Obj_AI_Base;
                        Events.AaStacks = 1;
                    }
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            WallQ.Drawing_OnDraw();

            if (DrawMenu["drawStacks"].Cast<CheckBox>().CurrentValue && Events.AAedTarget != null)
            {
                var color = new [] {Color.White, Color.Aqua}[Events.AaStacks - 1];
                new Circle() { Color = color, Radius = 200 }.Draw(Events.AAedTarget.Position);
            }
            if (DrawMenu["drawERange"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.White, Radius = E.Range }.Draw(_Player.Position);
            }
            if (DrawMenu["condemnVisualiser"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var point in Points)
                {
                    new Circle() {Color = Color.Red, Radius = 10}.Draw(point.To3D());
                }
                foreach (var point in CorrectPoints)
                {
                    new Circle()
                    {
                        Color = CorrectPoints.Count > 3 ? Color.DeepSkyBlue : Color.Green,
                        Radius = 10
                    }.Draw(point.To3D());
                }
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {

            Orbwalker.ForcedTarget = null;

            if (Events.AAedTarget == null || Events.LastAa + 3500 + 400 <= Environment.TickCount || Events.AAedTarget.IsDead || !Events.AAedTarget.HasBuff("vaynesilvereddebuff") && (Events.LastAa + 1000 < Environment.TickCount))
            {
                Events.AAedTarget = null;
                Events.AaStacks = 0;
            }

            if (DrawMenu["wallJumpKey"].Cast<KeyBind>().CurrentValue)
            {
                WallQ.WallTumble();
            }
            if (CondemnPriorityMenu["autoCondemnToggle"].Cast<KeyBind>().CurrentValue)
            {
                var condemnTarget = Condemn.CondemnTarget();
                if (condemnTarget != null)
                {
                    E.Cast(condemnTarget);
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                States.Combo();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                States.Harass();
            }
        }

        public static bool Has2WStacks(this AIHeroClient target)
        {
            return target.Buffs.Any(bu => bu.Name == "vaynesilvereddebuff");
        }

    }
}
