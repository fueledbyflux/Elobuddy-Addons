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

        public static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (e.End.Distance(_Player) < 200 && sender.IsValidTarget())
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
            return;
        }

        public static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
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
