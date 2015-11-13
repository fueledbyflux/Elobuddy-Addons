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

        public static void ComboAfterAa(Obj_AI_Base target)
        {
            if (Player.Instance.HasBuff("RivenFengShuiEngine") && Riven.R.IsReady())
            {
                if (Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, (float)(DamageHandler.RDamage(target))) + Player.Instance.GetAutoAttackDamage(target, true) > target.Health)
                {
                    Riven.R.Cast(target);
                    return;
                }
            }
            if (Riven.W.IsReady() && Riven.W.IsInRange(target))
            {
                if (ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
                {
                    ItemHandler.Hydra.Cast();
                    return;
                }
                Player.CastSpell(SpellSlot.W);
                return;
            }
            if (Riven.Q.IsReady())
            {
                Player.CastSpell(SpellSlot.Q, target.Position);
                return;
            }
            if (ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
            {
                ItemHandler.Hydra.Cast();
                return;
            }
        }

        public static void HarassAfterAa(Obj_AI_Base target)
        {

        }

        public static void LastHitAfterAa(Obj_AI_Base target)
        {
            var unitHp = target.Health - Player.Instance.GetAutoAttackDamage(target, true);
            if (unitHp > 0)
            {
                if (Riven.Q.IsReady() &&
                    Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, DamageHandler.QDamage()) >
                    unitHp)
                {
                    Player.CastSpell(SpellSlot.Q, target.Position);
                    return;
                }
                if (Riven.W.IsReady() && Riven.W.IsInRange(target) &&
                    Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, DamageHandler.WDamage()) >
                    unitHp)
                {
                    Player.CastSpell(SpellSlot.W);
                    return;
                }
            }
        }

        public static void LaneClearAfterAa(Obj_AI_Base target)
        {
            var unitHp = target.Health - Player.Instance.GetAutoAttackDamage(target, true);
            if (unitHp > 0)
            {
                if (Riven.Q.IsReady())
                {
                    Player.CastSpell(SpellSlot.Q, target.Position);
                }
                if (Riven.W.IsReady() && Riven.W.IsInRange(target))
                {
                    Player.CastSpell(SpellSlot.W);
                    return;
                }
            }
        }

        public static void JungleAfterAa(Obj_AI_Base target)
        {
            if (Riven.W.IsReady() && Riven.W.IsInRange(target))
            {
                if (ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
                {
                    ItemHandler.Hydra.Cast();
                    return;
                }
                Player.CastSpell(SpellSlot.W);
                return;
            }
            if (Riven.Q.IsReady())
            {
                Player.CastSpell(SpellSlot.Q, target.Position);
                return;
            }
            if (ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
            {
                ItemHandler.Hydra.Cast();
                return;
            }
        }
    }
}