using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace JinxBuddy
{
    internal class Program
    {
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static Dictionary<SpellSlot, Spell.SpellBase> Spells = new Dictionary<SpellSlot, Spell.SpellBase>()
        {
            {SpellSlot.Q, new Spell.Active(SpellSlot.Q)},
            {SpellSlot.W, new Spell.Skillshot(SpellSlot.W, 1450, SkillShotType.Linear, 600, 3300, 120)},
            {SpellSlot.E, new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Circular, 1200, 1750, 1)},
            {SpellSlot.R, new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 600, 1700, 140)}
        };

        public static Dictionary<SpellSlot, int> Mana = new Dictionary<SpellSlot, int>()
        {
            {SpellSlot.Q, 20},
            {SpellSlot.W, new[] {50, 60, 70, 80, 90}[Spells[SpellSlot.W].IsLearned ? Spells[SpellSlot.W].Level - 1 : 0]},
            {SpellSlot.E, 50},
            {SpellSlot.R, 100}
        };

        public static int WMana
        {
            get
            {
                return new[] {50, 60, 70, 80, 90}[Spells[SpellSlot.W].IsLearned ? Spells[SpellSlot.W].Level - 1 : 0];
            }
        }

        public static int RMana = 100;
        public static int EMana = 50;
        public static int QMana = 20;

        public static Menu menu, ComboMenu, HarassMenu, FarmMenu;
        public static CheckBox SmartMode;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Bootstrap.Init(null);

            menu = MainMenu.AddMenu("Jinx Buddy", "jinxBuddy");
            menu.AddGroupLabel("Jinx Buddy");
            menu.AddLabel("made by the flauxbauss");
            menu.AddSeparator();
            SmartMode = menu.Add("smartMode", new CheckBox("Smart Mode", false));
            menu.AddLabel("Smart mode enables mana management / smarter skill usage");

            ComboMenu = menu.AddSubMenu("Combo", "ComboJinx");
            ComboMenu.Add("useQCombo", new CheckBox("Use Q"));
            ComboMenu.Add("useWCombo", new CheckBox("Use W"));
            ComboMenu.Add("useECombo", new CheckBox("Use E"));
            ComboMenu.Add("useRCombo", new CheckBox("Use R"));

            ComboMenu = menu.AddSubMenu("Harass", "HarassJinx");
            ComboMenu.Add("useQHarass", new CheckBox("Use Q"));
            ComboMenu.Add("useWHarass", new CheckBox("Use W"));

            ComboMenu = menu.AddSubMenu("Farm", "FarmJinx");
            ComboMenu.Add("useQFarm", new CheckBox("Use Q"));

            Events.Init();
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {

        }

        private static void Game_OnTick(EventArgs args)
        {
            Orbwalker.ForcedTarget = null;
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) Combo();
            else if(Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) Harass();
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) WaveClear();
        }

        public static void WaveClear()
        {
            var minions = ObjectManager.Get<Obj_AI_Base>().Where(a => a.IsEnemy && a.Distance(_Player) <= Events.MinigunRange(a) + Events.FishBonesBonus && a.Health <= _Player.GetAutoAttackDamage(a) * 1.1);
            var minions2 =
                ObjectManager.Get<Obj_AI_Base>()
                    .Where(a => a.IsEnemy && a.Distance(_Player) <= Events.MinigunRange(a) + Events.FishBonesBonus + 100 && a.Health <= _Player.GetAutoAttackDamage(a) * 1.1);
            var minion = minions.OrderByDescending(a => minions2.Count(b => b.Distance(a) <= 200)).FirstOrDefault();
            Orbwalker.ForcedTarget = minion;
        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(1550, DamageType.Physical);

            if (target == null) return;

            if (Orbwalker.IsAutoAttacking) return;

            if (HarassMenu["useQHarass"].Cast<CheckBox>().CurrentValue && Spells[SpellSlot.Q].IsReady())
            {
                if (target.Distance(_Player) <= Events.MinigunRange(target) && (Events.FishBonesActive || (_Player.ManaPercent > 40 && SmartMode.CurrentValue)))
                {
                    Spells[SpellSlot.Q].Cast();
                    return;
                }
                else if ((target.Distance(_Player) <= Events.MinigunRange(target) + Events.FishBonesBonus) &&
                         !Events.FishBonesActive)
                {
                    Spells[SpellSlot.Q].Cast();
                    return;
                }
            }

            if (HarassMenu["useWHarass"].Cast<CheckBox>().CurrentValue && Spells[SpellSlot.W].IsReady())
            {
                var pred = Prediction.Position.PredictLinearMissile(target, Spells[SpellSlot.W].Range, 60, 600, 3300, 0);
                if (SmartMode.CurrentValue && pred.HitChance >= HitChance.High)
                {
                    if (_Player.Mana > RMana + EMana + WMana)
                    {
                        Spells[SpellSlot.W].Cast(pred.CastPosition);
                        return;
                    }
                }
                else
                {
                    Spells[SpellSlot.W].Cast(pred.CastPosition);
                    return;
                }
            }


        }

        public static void Combo()
        {
            var target = TargetSelector.GetTarget(1550, DamageType.Physical);
            var rtarget = TargetSelector.GetTarget(3000, DamageType.Physical);


            if (target == null) return;

            if (Orbwalker.IsAutoAttacking) return;

            if (ComboMenu["useRCombo"].Cast<CheckBox>().CurrentValue && Spells[SpellSlot.R].IsReady() && rtarget.Distance(_Player) > _Player.GetAutoAttackRange(rtarget) && rtarget.IsKillableByR() || Spells[SpellSlot.R].IsReady() && _Player.HealthPercent <= 30 && rtarget.IsKillableByR())
            {
                Spells[SpellSlot.R] = new Spell.Skillshot(SpellSlot.R, 2000, SkillShotType.Linear, 600, (int) UltimateHandler.UltSpeed(target.Position), 140);
                Spells[SpellSlot.R].Cast(rtarget);
            }

            if (ComboMenu["useECombo"].Cast<CheckBox>().CurrentValue && Spells[SpellSlot.E].IsReady() && target.HasBuffOfType(BuffType.Slow)
                || target.HasBuffOfType(BuffType.Stun)
                || target.HasBuffOfType(BuffType.Snare)
                || target.HasBuffOfType(BuffType.Suppression))
            {
                Spells[SpellSlot.E].Cast(target);
            }

            if (ComboMenu["useQCombo"].Cast<CheckBox>().CurrentValue && Spells[SpellSlot.Q].IsReady())
            {
                var distance = target.Distance(_Player);
                if (Events.FishBonesActive)
                {
                    if (distance < Events.MinigunRange(target))
                    {
                        Spells[SpellSlot.Q].Cast();
                    }
                }
                else
                {
                    if (distance > Events.MinigunRange(target) &&
                        distance <= Events.MinigunRange(target) + Events.FishBonesBonus)
                    {
                        Spells[SpellSlot.Q].Cast();
                    }
                }
            }

            if (ComboMenu["useWCombo"].Cast<CheckBox>().CurrentValue && Spells[SpellSlot.W].IsReady())
            {
                var pred = Prediction.Position.PredictLinearMissile(target, Spells[SpellSlot.W].Range, 60, 600, 3300, 0);
                if (SmartMode.CurrentValue && pred.HitChance >= HitChance.High && !pred.CollisionObjects.Any())
                {
                    if (_Player.Mana > RMana + EMana)
                    {
                        Spells[SpellSlot.W].Cast(pred.CastPosition);
                    }
                }
                else
                {
                    Spells[SpellSlot.W].Cast(pred.CastPosition);
                }
            }

        }
    }
}
