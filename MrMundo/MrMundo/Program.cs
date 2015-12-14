using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using MrMundo.States;
using SharpDX;

namespace MrMundo
{
    internal class Program
    {
        public static Menu Menu, ComboMenu, HarassMenu, FarmMenu, DrawMenu;
        public static int ResetTime;

        public static bool HasW
        {
            get { return Player.HasBuff("BurningAgony") || ResetTime > Environment.TickCount; }
        }

        private static void Main(string[] args)
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


            DrawMenu = Menu.AddSubMenu("Drawing", "mrmundoDrawing", "Mr Mundo - Drawing");
            DrawMenu.Add("drawQ", new CheckBox("Draw Q", false));
            DrawMenu.Add("drawW", new CheckBox("Draw W", false));
            DrawMenu.Add("drawE", new CheckBox("Draw E", false));

            StateHandler.Init();
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Color.LawnGreen, SpellHandler.Q.Range, Player.Instance.Position);
            }
            if (DrawMenu["drawW"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Color.LimeGreen, SpellHandler.W.Range, Player.Instance.Position);
            }
            if (DrawMenu["drawE"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Color.LightBlue, SpellHandler.E.Range, Player.Instance.Position);
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.SData.Name == Player.GetSpell(SpellSlot.E).Name)
            {
                Console.WriteLine(Player.GetSpell(SpellSlot.E).Name);
                Orbwalker.ResetAutoAttack();
            }
            if (args.SData.Name == Player.GetSpell(SpellSlot.W).Name && !Player.HasBuff("BurningAgony"))
            {
                ResetTime = Environment.TickCount + 500;
            }
        }

        public static float QDamage(Obj_AI_Base target)
        {
            var level = Player.GetSpell(SpellSlot.Q).Level;
            if (level < 1) return 0;
            var value = new[]
            {
                (new[] {80, 130, 180, 230, 280}[level - 1]),
                (int) (new[] {0.15, 0.175, 0.21, 0.225, 0.25}[level - 1]*target.Health)
            }.Max();
            if (EntityManager.Heroes.Enemies.Any(a => a.NetworkId == target.NetworkId))
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, value);

            var maxMonsters = new[] {300, 350, 400, 450, 500}[level - 1];
            if (maxMonsters < value)
            {
                value = maxMonsters;
            }
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, value);
        }
    }
}