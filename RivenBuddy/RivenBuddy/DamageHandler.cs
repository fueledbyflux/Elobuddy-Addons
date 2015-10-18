using System;
using EloBuddy;
using EloBuddy.SDK;

namespace RivenBuddy
{
    internal class DamageHandler
    {
        public static double ComboDamage(Obj_AI_Base target, bool noR = false)
        {
            double dmg = 0;
            var passiveStacks = 0;

            dmg += SpellManager.Spells[SpellSlot.Q].IsReady()
                ? QDamage(!noR)*(3 - SpellEvents.QCount)
                : 0;
            passiveStacks += SpellManager.Spells[SpellSlot.Q].IsReady()
                ? (3 - SpellEvents.QCount)
                : 0;

            dmg += SpellManager.Spells[SpellSlot.W].IsReady()
                ? WDamage()
                : 0;
            passiveStacks += SpellManager.Spells[SpellSlot.W].IsReady()
                ? 1
                : 0;
            passiveStacks += SpellManager.Spells[SpellSlot.E].IsReady()
                ? 1
                : 0;

            dmg += PassiveDamage()*passiveStacks;
            dmg += (SpellManager.Spells[SpellSlot.R].IsReady() && !noR && !SpellEvents.HasR ? Player.Instance.TotalAttackDamage * 1.2 : Player.Instance.TotalAttackDamage)*passiveStacks;

            if (dmg < 10)
            {
                return 3*Player.Instance.TotalAttackDamage;
            }

            dmg += SpellManager.Spells[SpellSlot.R].IsReady() && !noR ? RDamage(target, Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, (float) dmg)) : 0;
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, (float) dmg);
        }

        public static double QDamage(bool useR = false)
        {
            return new double[] {10, 30, 50, 70, 90}[SpellManager.Spells[SpellSlot.Q].Level - 1] +
                   ((SpellManager.Spells[SpellSlot.R].IsReady() && useR && !SpellEvents.HasR ? Player.Instance.TotalAttackDamage * 1.2 : Player.Instance.TotalAttackDamage) /100)*
                   new double[] {40, 45, 50, 55, 60}[SpellManager.Spells[SpellSlot.Q].Level - 1];
        }

        public static double WDamage()
        {
            return new double[] {50, 80, 110, 140, 170}[SpellManager.Spells[SpellSlot.W].Level - 1] +
                   1*ObjectManager.Player.FlatPhysicalDamageMod;
        }

        public static double PassiveDamage()
        {
            return ((20 + ((Math.Floor((double) ObjectManager.Player.Level/3))*5))/100)*
                   (ObjectManager.Player.BaseAttackDamage + ObjectManager.Player.FlatPhysicalDamageMod);
        }

        public static double RDamage(Obj_AI_Base target, double healthMod = 0)
        {
            if (!SpellManager.Spells[SpellSlot.R].IsLearned) return 0;
            var hpPercent = (target.Health - healthMod > 0 ? 1 : target.Health - healthMod) /target.MaxHealth;
            return (new double[] {80, 120, 160}[SpellManager.Spells[SpellSlot.R].Level - 1]
                   + 0.6 * Player.Instance.FlatPhysicalDamageMod) *
                   (hpPercent < 25 ? 3 : (((100 - hpPercent) * 2.67) / 100) + 1);
        }
    }
}