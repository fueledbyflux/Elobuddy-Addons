using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace JinxBuddy
{
    class Events
    {
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static float FishBonesBonus
        {
            get { return 75f + 25f*Program.Spells[SpellSlot.Q].Level; }
        }

        public static float MinigunRange(Obj_AI_Base target = null)
        {
            return (525 + _Player.BoundingRadius + (target != null ? target.BoundingRadius : 0));
        }

        public static bool FishBonesActive
        {
            get { return _Player.AttackRange > 525; }
        }

        public const int AoeRadius = 200;

        public static void Init()
        {
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
        }



        private static long _lastChange = Environment.TickCount;

        private static void Gapcloser_OnGapCloser(AIHeroClient sender, GapCloserEventArgs e)
        {
            if (sender.IsEnemy && e.End.Distance(_Player) < 200)
            {
                Program.Spells[SpellSlot.E].Cast(e.End);
            }
        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.None  || _lastChange + 300 > Environment.TickCount) return;
            var selectedUnit = target as AIHeroClient;
            if (selectedUnit != null && (Program.HarassMenu["useQHarass"].Cast<CheckBox>().CurrentValue && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) || Program.ComboMenu["useQCombo"].Cast<CheckBox>().CurrentValue && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)))
            {
                if (selectedUnit.CountEnemiesInRange(AoeRadius) > 2 || _Player.GetAutoAttackDamage(selectedUnit) * 1.1 >= target.Health + target.AttackShield && _Player.GetAutoAttackDamage(selectedUnit) < target.Health + target.AttackShield)
                {
                    if (!FishBonesActive)
                    {
                        Program.Spells[SpellSlot.Q].Cast();
                        _lastChange = Environment.TickCount;
                    }
                }
                else
                {
                    if (FishBonesActive && target.Distance(_Player) < MinigunRange(selectedUnit))
                    {
                        Program.Spells[SpellSlot.Q].Cast();
                        _lastChange = Environment.TickCount;
                    }
                }
            }
            else
            {
                var minion = target as Obj_AI_Base;
                if (minion != null && minion.IsMinion && Program.FarmMenu["useQFarm"].Cast<CheckBox>().CurrentValue)
                {
                    var count =
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Count(
                                a =>
                                    a.Health < _Player.GetAutoAttackDamage(a) * 1.1 && a.Distance(target) < AoeRadius &&
                                    a.IsValidTarget());
                    if (FishBonesActive && count < 2 && target.Distance(_Player) < MinigunRange(minion))
                    {
                        Program.Spells[SpellSlot.Q].Cast();
                        _lastChange = Environment.TickCount;
                    }
                    else if (!FishBonesActive && count >= 2)
                    {
                        Program.Spells[SpellSlot.Q].Cast();
                        _lastChange = Environment.TickCount;
                    }
                }
            }
            args.Process = true;
        }
    }
}
