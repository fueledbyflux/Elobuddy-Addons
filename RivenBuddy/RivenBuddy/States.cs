using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace RivenBuddy
{
    internal class States
    {
        public static Obj_AI_Base Target;
        public static SpellDataInst Flash;

        public static void Burst()
        {
            var target = TargetSelector.GetTarget(SpellManager.Spells[SpellSlot.E].Range + SpellManager.Spells[SpellSlot.W].Range + 400, DamageType.Physical);

            Orbwalker.ForcedTarget = target;
            Orbwalker.OrbwalkTo(Game.CursorPos);

            if (target == null)
            {
                Target = null;
                Queuer.Queue.Clear();
                return;
            }
            if (Target == null || target.NetworkId != Target.NetworkId)
            {
                Target = target;
                Queuer.Queue.Clear();
            }
            if (Queuer.Queue.Any())
            {
                Queuer.DoQueue(target);
                return;
            }

            if (Program.ComboMenu["burst.flash"].Cast<CheckBox>().CurrentValue && SpellManager.Spells[SpellSlot.Q].IsReady() &&
                SpellManager.Spells[SpellSlot.W].IsReady() &&
                SpellManager.Spells[SpellSlot.E].IsReady() &&
                SpellManager.Spells[SpellSlot.R].IsReady() &&
                !SpellEvents.HasR && Flash != null && Flash.IsReady)
            {
                Queuer.Queue.Add("E");
                Queuer.Queue.Add("R1");
                Queuer.Queue.Add("FL");
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("R2");
                Queuer.Queue.Add("Q");
                return;
            }
            if (target.IsValidTarget(SpellManager.Spells[SpellSlot.E].Range + SpellManager.Spells[SpellSlot.W].Range) && (!Program.ComboMenu["burst.flash"].Cast<CheckBox>().CurrentValue || Flash == null || !Flash.IsReady) &&
                SpellManager.Spells[SpellSlot.Q].IsReady() &&
                SpellManager.Spells[SpellSlot.W].IsReady() &&
                SpellManager.Spells[SpellSlot.E].IsReady() &&
                SpellManager.Spells[SpellSlot.R].IsReady() &&
                SpellEvents.QCount <= 1 && !SpellEvents.HasR)
            {
                Queuer.Queue.Add("E");
                Queuer.Queue.Add("R1");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("R2");
            }
            Combo();
        }

        public static void Combo(bool state = true)
        {
            var target = TargetSelector.GetTarget(500, DamageType.Physical);

            if (target == null)
            {
                Target = null;
                Queuer.Queue.Clear();
                return;
            }
            if (Target == null || target.NetworkId != Target.NetworkId)
            {
                Target = target;
                Queuer.Queue.Clear();
            }
            if (Queuer.Queue.Any())
            {
                if (state) Queuer.DoQueue(target);
                return;
            }

            float health = state ? target.Health : target.Health - Player.Instance.GetAutoAttackDamage(target, true);

            var Q = Program.ComboMenu["combo.useQ"].Cast<CheckBox>().CurrentValue;
            var W = Program.ComboMenu["combo.useW"].Cast<CheckBox>().CurrentValue;
            var E = Program.ComboMenu["combo.useE"].Cast<CheckBox>().CurrentValue;
            var R1 = Program.ComboMenu["combo.useR"].Cast<CheckBox>().CurrentValue;
            var R2 = Program.ComboMenu["combo.useR2"].Cast<CheckBox>().CurrentValue;

            var comboDmg = DamageHandler.ComboDamage(target, true);
            if (Q && W && E && R1 && R2 &&
                SpellManager.Spells[SpellSlot.Q].IsReady() &&
                SpellManager.Spells[SpellSlot.W].IsReady() &&
                SpellManager.Spells[SpellSlot.E].IsReady() &&
                SpellManager.Spells[SpellSlot.R].IsReady() &&
                SpellEvents.QCount == 0 &&
                !SpellEvents.HasR2
                &&
                (Program.IsRActive ||
                 target.IsValidTarget(SpellManager.Spells[SpellSlot.E].Range + SpellManager.Spells[SpellSlot.W].Range)
                 && comboDmg < health &&
                 comboDmg +
                 Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                     (int) DamageHandler.RDamage(target, comboDmg)) >= health))
            {
                Queuer.Queue.Add("E");
                Queuer.Queue.Add("R1");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                return;
            }

            if (SpellManager.Spells[SpellSlot.R].IsReady() &&
                R1 &&
                (Program.IsRActive ||
                 comboDmg < health &&
                 comboDmg + Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                     (int) DamageHandler.RDamage(target, comboDmg)) >= health) && !SpellEvents.HasR)
            {
                if (Program.ComboMenu["combo.eR1"].Cast<CheckBox>().CurrentValue && SpellManager.Spells[SpellSlot.E].IsReady() && SpellManager.Spells[SpellSlot.R].IsReady())
                {
                    Queuer.Queue.Add("E");
                    Queuer.Queue.Add("R1");
                    return;
                }
                if (Program.ComboMenu["combo.R1"].Cast<CheckBox>().CurrentValue && SpellManager.Spells[SpellSlot.R].IsReady())
                {
                    Queuer.Queue.Add("R1");
                    return;
                }
            }

            if (SpellManager.Spells[SpellSlot.R].IsReady() && SpellEvents.HasR2 &&
                R2)
            {
                if (Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                    (int) DamageHandler.RDamage(target)) >= health)
                {
                    if (Program.ComboMenu["combo.eR2"].Cast<CheckBox>().CurrentValue &&
                        SpellManager.Spells[SpellSlot.E].IsReady() && SpellManager.Spells[SpellSlot.R].IsReady())
                    {
                        Queuer.Queue.Add("E");
                        Queuer.Queue.Add("R2");
                        return;
                    }
                    if (Program.ComboMenu["combo.R2"].Cast<CheckBox>().CurrentValue && SpellManager.Spells[SpellSlot.R].IsReady())
                    {
                        Queuer.Queue.Add("R2");
                        return;
                    }
                }
                if (Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                    (int) (DamageHandler.RDamage(target) + DamageHandler.QDamage())) >= health)
                {
                    if (Program.ComboMenu["combo.qR2"].Cast<CheckBox>().CurrentValue &&
                        SpellManager.Spells[SpellSlot.R].IsReady() && SpellManager.Spells[SpellSlot.Q].IsReady())
                    {
                        Queuer.Queue.Add("R2");
                        Queuer.Queue.Add("Q");
                        return;
                    }
                }
            }

            if (Q && W && E &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellManager.Spells[SpellSlot.E].IsReady() &&
                SpellManager.Spells[SpellSlot.W].IsReady() && Queuer.Tiamat != null && Queuer.Tiamat.CanUseItem()
                && target.IsValidTarget(SpellManager.Spells[SpellSlot.E].Range + SpellManager.Spells[SpellSlot.W].Range +
                                     ObjectManager.Player.BoundingRadius + target.BoundingRadius))
            {
                Queuer.Queue.Add("E");
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                return;
            }
            if (Q && W && E &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellManager.Spells[SpellSlot.E].IsReady() &&
                SpellManager.Spells[SpellSlot.W].IsReady() && (Queuer.Tiamat == null || !Queuer.Tiamat.CanUseItem())
                &&
                target.IsValidTarget(SpellManager.Spells[SpellSlot.E].Range +
                                     ObjectManager.Player.GetAutoAttackRange(target)))
            {
                Queuer.Queue.Add("E");
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                return;
            }
            if (Q && W &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellEvents.QCount == 2 &&
                SpellManager.Spells[SpellSlot.W].IsReady()
                &&
                target.IsValidTarget(SpellManager.Spells[SpellSlot.Q].Range +
                                     SpellManager.Spells[SpellSlot.W].GetTrueRange(target)))
            {
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                return;
            }
            if (W && SpellManager.Spells[SpellSlot.W].IsReady() && SpellManager.Spells[SpellSlot.W].InRange(target))
            {
                if (Program.ComboMenu["combo.hydra"].Cast<CheckBox>().CurrentValue) Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                return;
            }

            if (Q && SpellManager.Spells[SpellSlot.Q].IsReady() && Orbwalker.CanAutoAttack)
            {
                if (Player.Instance.IsInAutoAttackRange(target))
                {
                    Queuer.Queue.Add("AA");
                    Queuer.Queue.Add("Q");
                    return;
                }
            }
            if (!target.IsValidTarget(Player.Instance.GetAutoAttackRange(target)) &&
                target.IsValidTarget(Player.Instance.GetAutoAttackRange(target) + SpellManager.Spells[SpellSlot.Q].Range) && Program.ComboMenu["combo.useQGapClose"].Cast<CheckBox>().CurrentValue)
            {
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                return;
            }
            if (W && E && SpellManager.Spells[SpellSlot.E].IsReady() && SpellManager.Spells[SpellSlot.W].IsReady() &&
                target.IsValidTarget(SpellManager.Spells[SpellSlot.W].GetTrueRange(target) +
                                     SpellManager.Spells[SpellSlot.E].Range))
            {
                Queuer.Queue.Add("E");
                if (Program.ComboMenu["combo.hydra"].Cast<CheckBox>().CurrentValue) Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                return;
            }
            if (E &&
                SpellManager.Spells[SpellSlot.E].IsReady() &&
                !target.IsValidTarget(Player.Instance.GetAutoAttackRange(target)))
            {
                Queuer.Queue.Add("E");
            }
            else
            {
                Queuer.Queue.Clear();
            }
        }

        public static void Harass(bool state = true)
        {
            var target = TargetSelector.GetTarget(500, DamageType.Physical);

            if (target == null)
            {
                Target = null;
                Queuer.Queue.Clear();
                return;
            }
            if (Target == null || target.NetworkId != Target.NetworkId)
            {
                Target = target;
                Queuer.Queue.Clear();
            }

            if (Queuer.Queue.Any())
            {
                if (state) Queuer.DoQueue(target);
                return;
            }

            if (Program.HarassMenu["harass.useQ"].Cast<CheckBox>().CurrentValue &&
                Program.HarassMenu["harass.useW"].Cast<CheckBox>().CurrentValue &&
                Program.HarassMenu["harass.useE"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellManager.Spells[SpellSlot.E].IsReady() &&
                SpellManager.Spells[SpellSlot.W].IsReady() && (Queuer.Tiamat == null || !Queuer.Tiamat.CanUseItem())
                &&
                target.IsValidTarget(SpellManager.Spells[SpellSlot.E].Range +
                                     ObjectManager.Player.GetAutoAttackRange(target)))
            {
                Queuer.Queue.Add("E");
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                return;
            }
            if (Program.HarassMenu["harass.useQ"].Cast<CheckBox>().CurrentValue &&
                Program.HarassMenu["harass.useW"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellEvents.QCount == 2 &&
                SpellManager.Spells[SpellSlot.W].IsReady()
                &&
                target.IsValidTarget(SpellManager.Spells[SpellSlot.Q].Range +
                                     SpellManager.Spells[SpellSlot.W].GetTrueRange(target)))
            {
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("AA");
                return;
            }
            if (Program.HarassMenu["harass.useQ"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && Orbwalker.CanAutoAttack)
            {
                if (SpellManager.Spells[SpellSlot.Q].IsInRange(target))
                {
                    Queuer.Queue.Add("AA");
                    Queuer.Queue.Add("Q");
                    return;
                }
            }
            if (Program.HarassMenu["harass.useW"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.W].IsReady() && SpellManager.Spells[SpellSlot.W].InRange(target))
            {
                if(Program.HarassMenu["harass.hydra"].Cast<CheckBox>().CurrentValue) Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                return;
            }
            if (Program.HarassMenu["harass.useE"].Cast<CheckBox>().CurrentValue &&
                Program.HarassMenu["harass.useW"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.E].IsReady() && SpellManager.Spells[SpellSlot.W].IsReady() &&
                target.IsValidTarget(SpellManager.Spells[SpellSlot.W].GetTrueRange(target) +
                                     SpellManager.Spells[SpellSlot.E].Range))
            {
                Queuer.Queue.Add("E");
                Queuer.Queue.Add("W");
                return;
            }
            if (Program.HarassMenu["harass.useE"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.E].IsReady() &&
                !target.IsValidTarget(Player.Instance.GetAutoAttackRange(target)))
            {
                Queuer.Queue.Add("E");
            }
            else
            {
                Queuer.Queue.Clear();
            }
        }

        public static void WaveClear(bool state = true)
        {
            var target = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position,
                SpellManager.Spells[SpellSlot.Q].Range + 100).OrderBy(a => a.Health).FirstOrDefault();

            if (target == null)
            {
                Target = null;
                Queuer.Queue.Clear();
                return;
            }
            if (Target == null || target.NetworkId != Target.NetworkId)
            {
                Target = null;
                Queuer.Queue.Clear();
            }

            if (Queuer.Queue.Any())
            {
                if (state) Queuer.DoQueue(target);
                return;
            }

            if (Program.MinionClear["waveclear.useQ"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellManager.Spells[SpellSlot.Q].IsInRange(target))
            {
                Queuer.Queue.Add("Q");
                return;
            }

            if (Program.MinionClear["waveclear.useQ"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsInRange(target) && Orbwalker.CanAutoAttack && SpellManager.Spells[SpellSlot.Q].IsReady())
            {
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                return;
            }

            if (Program.MinionClear["waveclear.useW"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.W].IsReady() && target.IsValidTarget(SpellManager.Spells[SpellSlot.W].Range))
            {
                SpellManager.Spells[SpellSlot.W].Cast();
                return;
            }

            if (Queuer.Tiamat != null && Queuer.Tiamat.CanUseItem() && target.IsValidTarget(300) && Program.MinionClear["waveclear.hydra"].Cast<CheckBox>().CurrentValue)
            {
                Queuer.Tiamat.Cast();
            }
        }

        public static void LastHit(bool state = true)
        {
            var target = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position,
                SpellManager.Spells[SpellSlot.Q].Range).OrderBy(a => a.Health).FirstOrDefault();

            if (target == null)
            {
                Target = null;
                Queuer.Queue.Clear();
                return;
            }
            if (Target == null || target.NetworkId != Target.NetworkId)
            {
                Target = target;
                Queuer.Queue.Clear();
            }

            if (Queuer.Queue.Any())
            {
                if (state) Queuer.DoQueue(target);
                return;
            }

            if (Program.MinionClear["lasthit.useQ"].Cast<CheckBox>().CurrentValue &&
                Player.Instance.GetSpellDamage(target, SpellSlot.Q) > target.Health &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellManager.Spells[SpellSlot.Q].IsInRange(target))
            {
                Queuer.Queue.Add("Q");
                return;
            }

            if (Program.MinionClear["lasthit.useQ"].Cast<CheckBox>().CurrentValue &&
                Player.Instance.GetSpellDamage(target, SpellSlot.Q) + Player.Instance.GetAutoAttackDamage(target) >
                target.Health && SpellManager.Spells[SpellSlot.Q].IsReady() &&
                SpellManager.Spells[SpellSlot.Q].IsInRange(target) && Orbwalker.CanAutoAttack)
            {
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                return;
            }

            if (Program.MinionClear["lasthit.useW"].Cast<CheckBox>().CurrentValue &&
                Player.Instance.GetSpellDamage(target, SpellSlot.W) > target.Health &&
                SpellManager.Spells[SpellSlot.W].IsReady() && target.IsValidTarget(SpellManager.Spells[SpellSlot.W].Range))
            {
                Queuer.Queue.Add("W");
            }
        }

        public static void Jungle(bool state = true)
        {
            var target = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position,
                SpellManager.Spells[SpellSlot.Q].Range + 300).OrderByDescending(a => a.MaxHealth).FirstOrDefault();

            if (target == null)
            {
                Target = null;
                Queuer.Queue.Clear();
                return;
            }
            if (Target == null || target.NetworkId != Target.NetworkId)
            {
                Target = target;
                Queuer.Queue.Clear();
            }

            if (Queuer.Queue.Any())
            {
                if (state) Queuer.DoQueue(target);
                return;
            }


            if (Program.Jungle["jungle.useE"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.E].IsReady())
            {
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("E");
                if (Queuer.Tiamat != null && Queuer.Tiamat.CanUseItem())
                {
                    Queuer.Queue.Add("H");
                    Queuer.Queue.Add("AA");
                }
                return;
            }

            if (Program.Jungle["jungle.useQ"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellManager.Spells[SpellSlot.Q].IsInRange(target) && Orbwalker.CanAutoAttack)
            {
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                return;
            }

            if (Program.Jungle["jungle.useW"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.W].IsReady() && target.IsValidTarget(SpellManager.Spells[SpellSlot.W].Range))
            {
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                return;
            }
        }

        public static void Flee()
        {
            if ((SpellManager.Spells[SpellSlot.Q].IsReady() && SpellEvents.QCount == 2 || !SpellManager.Spells[SpellSlot.Q].IsReady()) && SpellManager.Spells[SpellSlot.E].IsReady())
            {
                Player.CastSpell(SpellSlot.E, Game.CursorPos);
                SpellEvents.LastCast["Q"] = Environment.TickCount;
            }
            if (SpellManager.Spells[SpellSlot.Q].IsReady() && (SpellEvents.QCount >= 2 && !SpellManager.Spells[SpellSlot.E].IsReady() || !SpellManager.Spells[SpellSlot.E].IsReady()))
            {
                Player.CastSpell(SpellSlot.Q, Game.CursorPos);
                SpellEvents.LastCast["Q"] = Environment.TickCount;
            }
            
        }
    }
}