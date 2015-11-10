using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace RivenBuddy
{
    internal class Program
    {
        public static Menu Menu, ComboMenu, HarassMenu, MinionClear, Jungle, DrawMenu, HumanizerMenu;
        public static Text Text = new Text("", new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold));
        public static DamageIndicator.DamageIndicator Indicator;
        public static Spell.Skillshot R2;

        public static bool IsRActive
        {
            get { return ComboMenu["useR"].Cast<KeyBind>().CurrentValue; }
        }

        public static bool BurstActive
        {
            get { return ComboMenu["burst"].Cast<KeyBind>().CurrentValue; }
        }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Riven) return;

            Menu = MainMenu.AddMenu("RivenBuddy", "rivenbuddy");
            Menu.AddGroupLabel("Riven Buddy");
            Menu.AddSeparator();
            Menu.AddLabel("By Fluxy");
            Menu.AddLabel("nixi waz here");

            ComboMenu = Menu.AddSubMenu("Combo Settings", "combosettingsRiven");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("combo.useQ", new CheckBox("Use Q"));
            ComboMenu.Add("combo.useQGapClose", new CheckBox("Use Q to Gapclose", false));
            ComboMenu.Add("combo.useW", new CheckBox("Use W"));
            ComboMenu.Add("combo.useE", new CheckBox("Use E"));
            ComboMenu.Add("combo.useR", new CheckBox("Use R"));
            ComboMenu.Add("combo.useR2", new CheckBox("Use R2"));
            ComboMenu.Add("combo.hydra", new CheckBox("Use Hydra/Tiamat"));
            ComboMenu.Add("useR", new KeyBind("Force R", false, KeyBind.BindTypes.PressToggle, 'T'));
            ComboMenu.AddSeparator();
            ComboMenu.AddLabel("R1 Combos");
            ComboMenu.Add("combo.eR1", new CheckBox("E -> R1"));
            ComboMenu.Add("combo.R1", new CheckBox("R1"));
            ComboMenu.AddSeparator();
            ComboMenu.AddLabel("R2 Combos");
            ComboMenu.Add("combo.eR2", new CheckBox("E -> R2"));
            ComboMenu.Add("combo.qR2", new CheckBox("R2 -> Q"));
            ComboMenu.Add("combo.R2", new CheckBox("R2"));
            ComboMenu.AddSeparator();
            ComboMenu.AddGroupLabel("Burst Combo");
            ComboMenu.Add("burst.flash", new CheckBox("Use Flash in Burst"));
            ComboMenu.Add("burst", new KeyBind("Burst", false, KeyBind.BindTypes.HoldActive, 'Y'));
            ComboMenu.AddSeparator();
            ComboMenu.AddGroupLabel("Misc");
            ComboMenu.Add("combo.keepQAlive", new CheckBox("Keep Q Alive"));
            ComboMenu.Add("combo.useRBeforeExpire", new CheckBox("Use R Before Expire"));
            ComboMenu.Add("combo.alwaysCancelQ", new CheckBox("Always Cancel Q", false));

            HarassMenu = Menu.AddSubMenu("Harass Settings", "harasssettingsRiven");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("harass.hydra", new CheckBox("Use Hydra/Tiamat"));
            HarassMenu.Add("harass.useQ", new CheckBox("Use Q"));
            HarassMenu.Add("harass.useW", new CheckBox("Use W"));
            HarassMenu.Add("harass.useE", new CheckBox("Use E"));

            MinionClear = Menu.AddSubMenu("Minion Clear Settings", "farmettingsRiven");
            MinionClear.AddGroupLabel("LastHit Settings");
            MinionClear.Add("lasthit.useQ", new CheckBox("Use Q"));
            MinionClear.Add("lasthit.useW", new CheckBox("Use W"));
            MinionClear.AddSeparator();
            MinionClear.AddGroupLabel("Wave Clear Settings");
            MinionClear.Add("waveclear.hydra", new CheckBox("Use Hydra/Tiamat"));
            MinionClear.Add("waveclear.useQ", new CheckBox("Use Q"));
            MinionClear.Add("waveclear.useW", new CheckBox("Use W"));

            Jungle = Menu.AddSubMenu("Jungle Settings", "jungleettingsRiven");
            Jungle.AddGroupLabel("Jungle Clear Settings");
            Jungle.Add("jungle.hydra", new CheckBox("Use Hydra/Tiamat"));
            Jungle.Add("jungle.useQ", new CheckBox("Use Q"));
            Jungle.Add("jungle.useW", new CheckBox("Use W"));
            Jungle.Add("jungle.useE", new CheckBox("Use E"));

            DrawMenu = Menu.AddSubMenu("Draw Settings", "drawsettingsRiven");
            DrawMenu.AddGroupLabel("Draw Settings");
            DrawMenu.Add("draw.Q", new CheckBox("Draw Q", false));
            DrawMenu.Add("draw.W", new CheckBox("Draw W", false));
            DrawMenu.Add("draw.E", new CheckBox("Draw E", false));
            DrawMenu.Add("draw.R", new CheckBox("Draw R", false));
            DrawMenu.Add("draw.Damage", new CheckBox("Draw Damage"));
            DrawMenu.Add("draw.Combo", new CheckBox("Write Current Combo", false));
            DrawMenu.Add("draw.rState", new CheckBox("Write R State"));

            HumanizerMenu = Menu.AddSubMenu("Humanizer Settings");
            HumanizerMenu.Add("humanizerQSlow", new Slider("Humanizer Q Slow", 0, 0, 200));

            R2 = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Cone, 250, 1600, 125);
            States.Flash = Player.Spells.FirstOrDefault(a => a.SData.Name == "summonerflash");

            Queuer.Tiamat =
                ObjectManager.Player.InventoryItems.FirstOrDefault(
                    a => a.Id == ItemId.Tiamat_Melee_Only || a.Id == ItemId.Ravenous_Hydra_Melee_Only);

            SpellEvents.Init();
            Drawing.OnDraw += Drawing_OnDraw;
            Player.OnIssueOrder += Player_OnIssueOrder;
            Indicator = new DamageIndicator.DamageIndicator();
            Shop.OnBuyItem += Shop_OnBuyItem;
            Shop.OnSellItem += Shop_OnSellItem;
            Shop.OnUndo += Shop_OnUndo;
            Player.OnSwapItem += Player_OnSwapItem;
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Player_OnSwapItem(AIHeroClient sender, PlayerSwapItemEventArgs args)
        {
            if (!sender.IsMe) return;
            Queuer.Tiamat =
                ObjectManager.Player.InventoryItems.FirstOrDefault(
                    a => a.Id == ItemId.Tiamat_Melee_Only || a.Id == ItemId.Ravenous_Hydra_Melee_Only);
        }

        private static void Shop_OnUndo(ShopUndoPurchaseEventArgs args)
        {
            Queuer.Tiamat =
                ObjectManager.Player.InventoryItems.FirstOrDefault(
                    a => a.Id == ItemId.Tiamat_Melee_Only || a.Id == ItemId.Ravenous_Hydra_Melee_Only);
        }

        private static void Shop_OnSellItem(AIHeroClient sender, ShopActionEventArgs args)
        {
            if (!sender.IsMe) return;
            Queuer.Tiamat =
                ObjectManager.Player.InventoryItems.FirstOrDefault(
                    a => a.Id == ItemId.Tiamat_Melee_Only || a.Id == ItemId.Ravenous_Hydra_Melee_Only);
        }

        private static void Shop_OnBuyItem(AIHeroClient sender, ShopActionEventArgs args)
        {
            if (!sender.IsMe) return;
            if (args.Id == (int) ItemId.Tiamat_Melee_Only || args.Id == (int) ItemId.Ravenous_Hydra_Melee_Only)
            {
                Queuer.Tiamat =
                    ObjectManager.Player.InventoryItems.FirstOrDefault(
                        a => a.Id == ItemId.Tiamat_Melee_Only || a.Id == ItemId.Ravenous_Hydra_Melee_Only);
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            SpellManager.UpdateSpells();
            SpellEvents.UpdateSpells();
            TickTask();
        }

        public static GameObject OrderTarget;
        public static Vector3 OrderPosition;
        public static GameObjectOrder Order;

        private static void Player_OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            if (!sender.IsMe) return;
            Order = args.Order;
            OrderPosition = args.TargetPosition;
            OrderTarget = args.Target;
        }

        public static void IssueLastOrder()
        {
            switch (Order)
            {
                    case GameObjectOrder.AttackUnit:
                    if (OrderTarget != null) Player.IssueOrder(Order, OrderTarget);
                    break;

                case GameObjectOrder.MoveTo:
                    Player.IssueOrder(Order, OrderPosition);
                    break;
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var pos = Drawing.WorldToScreen(Player.Instance.Position);
            if (DrawMenu["draw.rState"].Cast<CheckBox>().CurrentValue)
                Text.Draw("Forced R: " + IsRActive, Color.AliceBlue, (int) pos.X - 45,
                    (int) pos.Y + 40);

            if (DrawMenu["draw.Combo"].Cast<CheckBox>().CurrentValue)
            {
                var s = Queuer.Queue.Aggregate("", (current, variable) => current + (" " + variable));
                Drawing.DrawText(100, 100, Color.Wheat, s);
            }
            if (DrawMenu["draw.Q"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.Spells[SpellSlot.Q].IsReady() ? SharpDX.Color.Cyan : SharpDX.Color.OrangeRed,
                    SpellManager.Spells[SpellSlot.Q].Range, Player.Instance.Position);
            }
            if (DrawMenu["draw.W"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.Spells[SpellSlot.W].IsReady() ? SharpDX.Color.Cyan : SharpDX.Color.OrangeRed,
                    SpellManager.Spells[SpellSlot.W].Range, Player.Instance.Position);
            }
            if (DrawMenu["draw.E"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.Spells[SpellSlot.E].IsReady() ? SharpDX.Color.Cyan : SharpDX.Color.OrangeRed,
                    SpellManager.Spells[SpellSlot.E].Range, Player.Instance.Position);
            }
            if (DrawMenu["draw.R"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.Spells[SpellSlot.R].IsReady() ? SharpDX.Color.Cyan : SharpDX.Color.OrangeRed,
                    SpellManager.Spells[SpellSlot.R].Range, Player.Instance.Position);
            }
        }

        private static void TickTask()
        {
            Orbwalker.ForcedTarget = null;
            if (BurstActive)
            {
                States.Burst();
                return;
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                States.Combo();
                return;
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                States.Harass();
                return;
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                var target = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position,
                       SpellManager.Spells[SpellSlot.Q].Range + 300).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
                if (target != null)
                {
                    States.Jungle();
                    return;
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                var target = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position,
                    SpellManager.Spells[SpellSlot.Q].Range + 300).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) && target == null)
                {
                    States.WaveClear();
                    return;
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                States.LastHit();
                return;
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                States.Flee();
                return;
            }
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.None && !BurstActive && Queuer.Queue.Any())
            {
                Queuer.Queue.Clear();
            }
        }
    }
}