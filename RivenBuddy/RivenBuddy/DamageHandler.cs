using System;
using EloBuddy;

namespace RivenBuddy
{
    internal class DamageHandler
    {
        public static double ComboDamage()
        {
            double dmg = 0;
            var passiveStacks = 0;

            dmg += SpellManager.Spells[SpellSlot.Q].IsReady()
                ? QDamage()*(3 - SpellEvents.QCount)
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
            dmg += Player.Instance.TotalAttackDamage*passiveStacks;

            if (dmg < 10)
            {
                return 3*Player.Instance.TotalAttackDamage;
            }

            return dmg;
        }

        public static double QDamage()
        {
            return new double[] {10, 30, 50, 70, 90}[SpellManager.Spells[SpellSlot.Q].Level - 1] +
                   ((ObjectManager.Player.BaseAttackDamage + ObjectManager.Player.FlatPhysicalDamageMod)/100)*
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
            var health = (target.MaxHealth - (target.Health - healthMod)) > 0
                ? (target.MaxHealth - (target.Health - healthMod))
                : 1;
            return (new double[] {80, 120, 160}[SpellManager.Spells[SpellSlot.R].Level - 1] +
                    0.6*ObjectManager.Player.FlatPhysicalDamageMod)*(health/target.MaxHealth*2.67 + 1);
        }
    }
}