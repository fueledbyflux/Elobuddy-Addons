using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace VayneBuddy
{
    class Events
    {
        public static Obj_AI_Base AAedTarget = null;
        public static long LastAa;
        public static int AaStacks;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapCloserEventArgs e)
        {
            if (e.End.Distance(_Player.Position) < 200 && sender.IsValidTarget())
            {
                Program.E.Cast(sender);
            }
        }

        public static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Program.ComboMenu["useRCombo"].Cast<CheckBox>().CurrentValue && target.IsValidTarget() && target is AIHeroClient)
            {
                if (Program.ComboMenu["noRUnderTurret"].Cast<CheckBox>().CurrentValue &&
                    ((AIHeroClient)target).IsUnderAlliedTurret())
                {
                    return;
                }

                Program.R.Cast();
            }
        }

        public static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            LastAa = Environment.TickCount;
            if (_Player.Spellbook.GetSpell(SpellSlot.W).IsLearned)
            {
                if (AAedTarget != null && target.NetworkId == AAedTarget.NetworkId)
                {
                    switch (AaStacks)
                    {
                        case 0:
                            AaStacks = 1;
                            break;
                        case 1:
                            AaStacks = 2;
                            break;
                        case 2:
                            AaStacks = 0;
                            AAedTarget = null;
                            break;
                    }
                }
                else
                {
                    AAedTarget = target as Obj_AI_Base;
                    AaStacks = 1;
                }
            }


            if (target.IsValidTarget() && target is AIHeroClient &&
                (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && Program.ComboMenu["useQCombo"].Cast<CheckBox>().CurrentValue || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) && Program.ComboMenu["useQHarass"].Cast<CheckBox>().CurrentValue))
            {
                var pos = (_Player.Position.Extend(Game.CursorPos, 300).Distance(target) <=
                           _Player.GetAutoAttackRange(target) &&
                           _Player.Position.Extend(Game.CursorPos, 300).Distance(target) > 100
                    ? Game.CursorPos
                    : (_Player.Position.Extend(target.Position, 300).Distance(target) < 100)
                    ? target.Position
                    : new Vector3());

                if (!pos.IsValid()) return;
                
                Program.Q.Cast(pos);

                return;
            }
            if (_Player.ManaPercent >= Program.FarmMenu["MinManaQLHWC"].Cast<Slider>().CurrentValue && target is Obj_AI_Minion &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) &&
                Program.FarmMenu["useQLastHit"].Cast<CheckBox>().CurrentValue ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                Program.FarmMenu["useQWaveClear"].Cast<CheckBox>().CurrentValue)
            {
                Core.DelayAction(delegate
                {
                    var minion =
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(
                                a =>
                                    a.NetworkId != target.NetworkId && a.IsEnemy &&
                                    a.IsValidTarget(_Player.GetAutoAttackRange(a)) &&
                                    a.Health <=
                                    _Player.GetAutoAttackDamage(a, true) +
                                    _Player.CalculateDamageOnUnit(a, DamageType.Physical,
                                        (float)
                                            (new[] { 0.3, 0.35, 0.4, 0.45, 0.5 }[
                                                _Player.Spellbook.GetSpell(SpellSlot.Q).Level - 1]) *
                                        (_Player.TotalAttackDamage))).OrderBy(a => a.Health).FirstOrDefault();

                    if (target.IsDead && minion != null && minion.IsValidTarget())
                    {
                        Program.Q.Cast(_Player.Position.Extend(Game.CursorPos, 300).Distance(minion) <
                               _Player.GetAutoAttackRange(minion)
                            ? Game.CursorPos
                            : minion.Position);
                        Orbwalker.ForcedTarget = minion;
                    }
                }, 300);
            }
        }



        public static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, InterruptableSpellEventArgs e)
        {
            var dangerLevel =
                new[]
                {
                    DangerLevel.Low, DangerLevel.Medium,
                    DangerLevel.High,
                }[Program.InterruptorMenu["dangerLevel"].Cast<Slider>().CurrentValue - 1];

            if (dangerLevel == DangerLevel.Medium && e.DangerLevel.HasFlag(DangerLevel.High) ||
                dangerLevel == DangerLevel.Low && e.DangerLevel.HasFlag(DangerLevel.High) &&
                e.DangerLevel.HasFlag(DangerLevel.Medium))
                return;

            if (e.Sender.IsValidTarget())
            {
                Program.E.Cast(e.Sender);
            }
        }

    }
}
