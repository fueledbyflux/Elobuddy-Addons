using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace ProjectRiven
{
    internal class StateHandler
    {
        public static bool EnableR;

        public static void Combo()
        {
            EnableR = false;
            if (Orbwalker.IsAutoAttacking) return;

            var target = TargetSelector.GetTarget(Riven.E.Range + Riven.W.Range + 200, DamageType.Physical);

            if (Riven.R.IsReady() && Player.Instance.HasBuff("RivenFengShuiEngine") &&
                Riven.ComboMenu["Combo.R2"].Cast<CheckBox>().CurrentValue)
            {
                if (
                    EntityManager.Heroes.Enemies.Where(
                        enemy =>
                            enemy.IsValidTarget(Riven.R.Range) &&
                            enemy.Health <
                            Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Physical,
                                DamageHandler.RDamage(enemy))).Any(enemy => Riven.R.Cast(enemy)))
                {
                    return;
                }
            }

            if (target == null) return;

            if (Riven.ComboMenu["Combo.R"].Cast<CheckBox>().CurrentValue && Riven.R.IsReady() && !Player.Instance.HasBuff("RivenFengShuiEngine"))
            {
                if (Riven.ComboMenu["Combo.RCombo"].Cast<CheckBox>().CurrentValue &&
                    target.Health > DamageHandler.ComboDamage(target, true)
                    && target.Health < DamageHandler.ComboDamage(target)
                    && target.Health > Player.Instance.GetAutoAttackDamage(target, true)*2 ||
                    Riven.ComboMenu["Combo.RPeople"].Cast<CheckBox>().CurrentValue &&
                    Player.Instance.CountEnemiesInRange(600) > 1 || Riven.IsRActive)
                {
                    if (Riven.E.IsReady())
                    {
                        EnableR = true;
                    }
                }
            }

            if (Riven.ComboMenu["Combo.E"].Cast<CheckBox>().CurrentValue && (target.Distance(Player.Instance) > Riven.W.Range || Riven.IsRActive && Riven.R.IsReady()) && Riven.E.IsReady())
            {
                Player.CastSpell(SpellSlot.E, target.Position);
                return;
            }

            if (Riven.ComboMenu["Combo.W"].Cast<CheckBox>().CurrentValue &&
                target.Distance(Player.Instance) <= Riven.W.Range && Riven.W.IsReady())
            {
                if (ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
                {
                    ItemHandler.Hydra.Cast();
                    return;
                }
                Player.CastSpell(SpellSlot.W);
            }
        }

        public static void Harass()
        {
            if (Orbwalker.IsAutoAttacking) return;

            var target = TargetSelector.GetTarget(Riven.E.Range + Riven.W.Range, DamageType.Physical);

            if (target == null) return;

            if (Riven.HarassMenu["Harass.E"].Cast<CheckBox>().CurrentValue &&
                (target.Distance(Player.Instance) > Riven.W.Range &&
                 target.Distance(Player.Instance) < Riven.E.Range + Riven.W.Range ||
                 Riven.IsRActive && Riven.R.IsReady() &&
                 target.Distance(Player.Instance) < Riven.E.Range + 265 + Player.Instance.BoundingRadius) &&
                Riven.E.IsReady())
            {
                Player.CastSpell(SpellSlot.E, target.Position);
                return;
            }

            if (Riven.HarassMenu["Harass.W"].Cast<CheckBox>().CurrentValue &&
                target.Distance(Player.Instance) <= Riven.W.Range && Riven.W.IsReady())
            {
                if (ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
                {
                    ItemHandler.Hydra.Cast();
                    return;
                }
                Player.CastSpell(SpellSlot.W);
            }
        }

        public static void LaneClear()
        {
            Orbwalker.ForcedTarget = null;
            if (Orbwalker.IsAutoAttacking || EventHandler.LastCastQ + 260 > Environment.TickCount) return;
            foreach (
                var minion in EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.IsValidTarget(Riven.W.Range)))
            {
                if (Riven.FarmMenu["WaveClear.Q"].Cast<CheckBox>().CurrentValue && Riven.Q.IsReady() &&
                    minion.Health <=
                    Player.Instance.CalculateDamageOnUnit(minion, DamageType.Physical, DamageHandler.QDamage()))
                {
                    Player.CastSpell(SpellSlot.Q, minion.Position);
                    return;
                }
                if (Riven.FarmMenu["WaveClear.W"].Cast<CheckBox>().CurrentValue && Riven.W.IsReady() &&
                    minion.Health <=
                    Player.Instance.CalculateDamageOnUnit(minion, DamageType.Physical, DamageHandler.WDamage()))
                {
                    Player.CastSpell(SpellSlot.W);
                    return;
                }
            }
        }

        public static void LastHit()
        {
            Orbwalker.ForcedTarget = null;
            if (Orbwalker.IsAutoAttacking) return;

            foreach (
                var minion in EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.IsValidTarget(Riven.W.Range)))
            {
                if (Riven.FarmMenu["LastHit.Q"].Cast<CheckBox>().CurrentValue && Riven.Q.IsReady() &&
                    minion.Health <=
                    Player.Instance.CalculateDamageOnUnit(minion, DamageType.Physical, DamageHandler.QDamage()))
                {
                    Player.CastSpell(SpellSlot.Q, minion.Position);
                    return;
                }
                if (Riven.FarmMenu["LastHit.W"].Cast<CheckBox>().CurrentValue && Riven.W.IsReady() &&
                    minion.Health <=
                    Player.Instance.CalculateDamageOnUnit(minion, DamageType.Physical, DamageHandler.WDamage()))
                {
                    Player.CastSpell(SpellSlot.W);
                    return;
                }
            }
        }

        public static void Jungle()
        {
            var minion =
                EntityManager.MinionsAndMonsters.Monsters.OrderByDescending(a => a.MaxHealth)
                    .FirstOrDefault(a => a.Distance(Player.Instance) < Player.Instance.GetAutoAttackRange(a));

            if (minion == null) return;

            if (Riven.FarmMenu["Jungle.E"].Cast<CheckBox>().CurrentValue && (!Riven.W.IsReady() && !Riven.Q.IsReady() || Player.Instance.HealthPercent < 20) && Riven.E.IsReady() &&
                EventHandler.LastCastW + 750 < Environment.TickCount)
            {
                Player.CastSpell(SpellSlot.E, minion.Position);
            }
        }

        public static void ComboAfterAa(Obj_AI_Base target)
        {
            if (Player.Instance.HasBuff("RivenFengShuiEngine") && Riven.R.IsReady() &&
                Riven.ComboMenu["Combo.R2"].Cast<CheckBox>().CurrentValue)
            {
                if (Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, DamageHandler.RDamage(target)) + Player.Instance.GetAutoAttackDamage(target, true) > target.Health)
                {
                    Riven.R.Cast(target);
                    return;
                }
            }
            if (Riven.ComboMenu["Combo.W"].Cast<CheckBox>().CurrentValue && Riven.W.IsReady() &&
                Riven.W.IsInRange(target))
            {
                if (ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
                {
                    ItemHandler.Hydra.Cast();
                    return;
                }
                Player.CastSpell(SpellSlot.W);
                return;
            }
            if (Riven.ComboMenu["Combo.Q"].Cast<CheckBox>().CurrentValue && Riven.Q.IsReady())
            {
                Player.CastSpell(SpellSlot.Q, target.Position);
                return;
            }
            if (ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
            {
                ItemHandler.Hydra.Cast();
            }
        }

        public static void HarassAfterAa(Obj_AI_Base target)
        {
            if (Riven.HarassMenu["Harass.W"].Cast<CheckBox>().CurrentValue && Riven.W.IsReady() &&
                Riven.W.IsInRange(target))
            {
                if (ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
                {
                    ItemHandler.Hydra.Cast();
                    return;
                }
                Player.CastSpell(SpellSlot.W);
                return;
            }
            if (Riven.HarassMenu["Harass.Q"].Cast<CheckBox>().CurrentValue && Riven.Q.IsReady())
            {
                Player.CastSpell(SpellSlot.Q, target.Position);
                return;
            }
            if (ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
            {
                ItemHandler.Hydra.Cast();
            }
        }

        public static void LastHitAfterAa(Obj_AI_Base target)
        {
            var unitHp = target.Health - Player.Instance.GetAutoAttackDamage(target, true);
            if (unitHp > 0)
            {
                if (Riven.FarmMenu["LastHit.Q"].Cast<CheckBox>().CurrentValue && Riven.Q.IsReady() &&
                    Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, DamageHandler.QDamage()) >
                    unitHp)
                {
                    Player.CastSpell(SpellSlot.Q, target.Position);
                    return;
                }
                if (Riven.FarmMenu["LastHit.W"].Cast<CheckBox>().CurrentValue && Riven.W.IsReady() &&
                    Riven.W.IsInRange(target) &&
                    Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, DamageHandler.WDamage()) >
                    unitHp)
                {
                    Player.CastSpell(SpellSlot.W);
                }
            }
        }

        public static void LaneClearAfterAa(Obj_AI_Base target)
        {
            var unitHp = target.Health - Player.Instance.GetAutoAttackDamage(target, true);
            if (unitHp > 0)
            {
                if (Riven.FarmMenu["WaveClear.Q"].Cast<CheckBox>().CurrentValue && Riven.Q.IsReady())
                {
                    Player.CastSpell(SpellSlot.Q, target.Position);
                    return;
                }
                if (Riven.FarmMenu["WaveClear.W"].Cast<CheckBox>().CurrentValue && Riven.W.IsReady() &&
                    Riven.W.IsInRange(target))
                {
                    Player.CastSpell(SpellSlot.W);
                    return;
                }
            }
            else
            {
                List<Obj_AI_Minion> minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.Position, Riven.Q.Range).Where(a => a.NetworkId != target.NetworkId).ToList();
                if (Riven.FarmMenu["WaveClear.Q"].Cast<CheckBox>().CurrentValue && Riven.Q.IsReady() && minions.Any())
                {
                    Player.CastSpell(SpellSlot.Q, minions[0].Position);
                    return;
                }
                minions = minions.Where(a => a.Distance(Player.Instance) < Riven.W.Range).ToList();
                if (Riven.FarmMenu["WaveClear.W"].Cast<CheckBox>().CurrentValue && Riven.W.IsReady() &&
                    Riven.W.IsInRange(target) && minions.Any())
                {
                    Player.CastSpell(SpellSlot.W);
                    return;
                }
            }
        }

        public static void JungleAfterAa(Obj_AI_Base target)
        {
            if (Riven.FarmMenu["Jungle.W"].Cast<CheckBox>().CurrentValue && Riven.W.IsReady() &&
                Riven.W.IsInRange(target))
            {
                if (ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
                {
                    ItemHandler.Hydra.Cast();
                    return;
                }
                Player.CastSpell(SpellSlot.W);
                return;
            }
            if (Riven.FarmMenu["Jungle.Q"].Cast<CheckBox>().CurrentValue && Riven.Q.IsReady())
            {
                Player.CastSpell(SpellSlot.Q, target.Position);
                return;
            }
            if (ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
            {
                ItemHandler.Hydra.Cast();
            }
        }
    }
}