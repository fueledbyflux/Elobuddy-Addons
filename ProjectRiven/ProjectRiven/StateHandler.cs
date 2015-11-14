using System;
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
                if (EntityManager.Heroes.Enemies.Where(enemy => enemy.IsValidTarget(Riven.R.Range) && enemy.Health < Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Physical, DamageHandler.RDamage(enemy))).Any(enemy => Riven.R.Cast(enemy)))
                {
                    return;
                }
            }

            if (target == null) return;

            if ((target.Distance(Player.Instance) > Riven.W.Range || Riven.IsRActive && Riven.R.IsReady()) && Riven.E.IsReady())
            {
                Player.CastSpell(SpellSlot.E, target.Position);
                return;
            }

            if (target.Distance(Player.Instance) <= Riven.W.Range && Riven.W.IsReady())
            {
                Player.CastSpell(SpellSlot.W);
            }
        }

        public static void Harass()
        {
            if (Orbwalker.IsAutoAttacking) return;

            var target = TargetSelector.GetTarget(Riven.E.Range + Riven.W.Range, DamageType.Physical);

            if (target == null) return;

            if ((target.Distance(Player.Instance) > Riven.W.Range && target.Distance(Player.Instance) < Riven.E.Range + Riven.W.Range || Riven.IsRActive && Riven.R.IsReady() && target.Distance(Player.Instance) < Riven.E.Range + 265 + Player.Instance.BoundingRadius) && Riven.E.IsReady())
            {
                Player.CastSpell(SpellSlot.E, target.Position);
                return;
            }

            if (target.Distance(Player.Instance) <= Riven.W.Range && Riven.W.IsReady())
            {
                Player.CastSpell(SpellSlot.W);
            }
        }

        public static void LaneClear()
        {
            Orbwalker.ForcedTarget = null;
            if (Orbwalker.IsAutoAttacking) return;
            foreach (var minion in EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.IsValidTarget(Riven.W.Range)))
            {
                if (Riven.Q.IsReady() && minion.Health <=
                    Player.Instance.GetAutoAttackDamage(minion, true) +
                    Player.Instance.CalculateDamageOnUnit(minion, DamageType.Physical, DamageHandler.QDamage()))
                {
                    Orbwalker.ForcedTarget = minion;
                    return;
                }
                if (Riven.W.IsReady() && !Riven.Q.IsReady() && minion.Health <=
                    Player.Instance.GetAutoAttackDamage(minion, true) +
                    Player.Instance.CalculateDamageOnUnit(minion, DamageType.Physical, DamageHandler.WDamage()))
                {
                    Orbwalker.ForcedTarget = minion;
                    return;
                }
            }
        }

        public static void LastHit()
        {
            Orbwalker.ForcedTarget = null;
            if (Orbwalker.IsAutoAttacking) return;

            foreach (var minion in EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.IsValidTarget(Riven.W.Range)))
            {
                if (Riven.Q.IsReady() && minion.Health <= Player.Instance.CalculateDamageOnUnit(minion, DamageType.Physical, DamageHandler.QDamage()))
                {
                    Player.CastSpell(SpellSlot.Q, minion.Position);
                    return;
                }
                if (Riven.W.IsReady() && minion.Health <= Player.Instance.CalculateDamageOnUnit(minion, DamageType.Physical, DamageHandler.WDamage()))
                {
                    Player.CastSpell(SpellSlot.W);
                    return;
                }
            }
        }

        public static void Jungle()
        {
            var minion = EntityManager.MinionsAndMonsters.Monsters.OrderByDescending(a => a.MaxHealth).FirstOrDefault(a => a.Distance(Player.Instance) < Player.Instance.GetAutoAttackRange(a));

            if (minion == null) return;

            if (!Riven.W.IsReady() && Riven.E.IsReady() && EventHandler.LastCastW + 750 < Environment.TickCount)
            {
                Player.CastSpell(SpellSlot.E, minion.Position);
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