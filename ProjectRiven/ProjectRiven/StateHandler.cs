using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace ProjectRiven
{
    internal class StateHandler
    {
        public static void Combo()
        {
            if (Orbwalker.IsAutoAttacking) return;

            var target = TargetSelector.GetTarget(Riven.E.Range + Riven.W.Range, DamageType.Physical);

            if (Riven.R.IsReady() && Player.Instance.HasBuff("RivenFengShuiEngine"))
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(enemy => enemy.IsValidTarget(Riven.R.Range)
                && enemy.Health < Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Physical, DamageHandler.RDamage(target))))
                {
                    if(Riven.R.Cast(enemy)) return;
                }
            }

            if (target == null) return;

            if (target.Distance(Player.Instance) > Riven.W.Range)
            {
                Player.CastSpell(SpellSlot.E, target.Position);
                return;
            }

            if (target.Distance(Player.Instance) <= Riven.W.Range)
            {
                Player.CastSpell(SpellSlot.W);
            }
        }
    }
}