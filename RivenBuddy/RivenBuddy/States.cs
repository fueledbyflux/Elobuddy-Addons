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

        public static void Burst()
        {
            var target = TargetSelector2.GetTarget(750, DamageType.Physical);

            Orbwalker.ForcedTarget = target;
            Orbwalker.OrbwalkTo(Game.CursorPos);

            if (target == null)
            {
                Target = null;
                Queuer.Queue = new List<string>();
                return;
            }
            if (Target == null || target.NetworkId != Target.NetworkId)
            {
                Target = target;
                Queuer.Queue = new List<string>();
            }
            if (Queuer.Queue.Any())
            {
                Queuer.DoQueue(target);
                return;
            }


            if (SpellManager.Spells[SpellSlot.Q].IsReady() &&
                SpellManager.Spells[SpellSlot.W].IsReady() &&
                SpellManager.Spells[SpellSlot.E].IsReady() &&
                SpellManager.Spells[SpellSlot.R].IsReady())
            {
                Queuer.Queue.Add("E");
                Queuer.Queue.Add("R1");
                Queuer.Queue.Add("FL");
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("R2");
                Queuer.Queue.Add("Q");
                return;
            }
            Combo();
        }

        public static void Combo()
        {
            // Queuer Stuff start

            var target = TargetSelector2.GetTarget(500, DamageType.Physical);

            if (target == null)
            {
                Target = null;
                Queuer.Queue = new List<string>();
                return;
            }
            if (Target == null || target.NetworkId != Target.NetworkId)
            {
                Target = target;
                Queuer.Queue = new List<string>();
            }
            if (Queuer.Queue.Any())
            {
                Queuer.DoQueue(target);
                return;
            }

            // Queuer Stuff End

            var comboDmg = Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                (int) DamageHandler.ComboDamage());
            if (Program.ComboMenu["combo.useQ"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useW"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useE"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useR"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useR2"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() &&
                SpellManager.Spells[SpellSlot.W].IsReady() &&
                SpellManager.Spells[SpellSlot.E].IsReady() &&
                SpellManager.Spells[SpellSlot.R].IsReady() &&
                SpellEvents.QCount == 0
                &&
                (Program.IsRActive ||
                 target.IsValidTarget(SpellManager.Spells[SpellSlot.E].Range + SpellManager.Spells[SpellSlot.W].Range)
                 && comboDmg < target.Health &&
                 comboDmg +
                 Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                     (int) DamageHandler.RDamage(target, comboDmg)) >= target.Health))
            {
                Queuer.Queue.Add("E");
                Queuer.Queue.Add("R1");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                return;
            }

            if (SpellManager.Spells[SpellSlot.R].IsReady() &&
                Program.ComboMenu["combo.useR"].Cast<CheckBox>().CurrentValue &&
                (Program.IsRActive ||
                 comboDmg < target.Health &&
                 comboDmg +
                 Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                     (int) DamageHandler.RDamage(target, comboDmg)) >= target.Health && !SpellEvents.HasR))
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

            if (SpellManager.Spells[SpellSlot.R].IsReady() &&
                Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                    (int) DamageHandler.RDamage(target)) >= target.Health && SpellEvents.HasR2 &&
                Program.ComboMenu["combo.useR2"].Cast<CheckBox>().CurrentValue)
            {
                if (Program.ComboMenu["combo.eR2"].Cast<CheckBox>().CurrentValue && SpellManager.Spells[SpellSlot.E].IsReady() && SpellManager.Spells[SpellSlot.R].IsReady())
                {
                    Queuer.Queue.Add("E");
                    Queuer.Queue.Add("R2");
                    return;
                }
                if (Program.ComboMenu["combo.qR2"].Cast<CheckBox>().CurrentValue && SpellManager.Spells[SpellSlot.R].IsReady() && SpellManager.Spells[SpellSlot.Q].IsReady())
                {
                    Queuer.Queue.Add("R2");
                    Queuer.Queue.Add("Q");
                    return;
                }
                if (Program.ComboMenu["combo.R2"].Cast<CheckBox>().CurrentValue && SpellManager.Spells[SpellSlot.R].IsReady())
                {
                    Queuer.Queue.Add("R2");
                    return;
                }
            }

            if (Program.ComboMenu["combo.useQ"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useW"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useE"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellManager.Spells[SpellSlot.E].IsReady() &&
                SpellManager.Spells[SpellSlot.W].IsReady() && Queuer.tiamat != null && Queuer.tiamat.CanUseItem()
                &&
                target.IsValidTarget(SpellManager.Spells[SpellSlot.E].Range + SpellManager.Spells[SpellSlot.W].Range +
                                     ObjectManager.Player.BoundingRadius + target.BoundingRadius))
            {
                Queuer.Queue.Add("E");
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("Q");
                return;
            }
            if (Program.ComboMenu["combo.useQ"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useW"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useE"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellManager.Spells[SpellSlot.E].IsReady() &&
                SpellManager.Spells[SpellSlot.W].IsReady() && (Queuer.tiamat == null || !Queuer.tiamat.CanUseItem())
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
            if (Program.ComboMenu["combo.useW"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useQ"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellEvents.QCount == 2 &&
                SpellManager.Spells[SpellSlot.W].IsReady()
                &&
                target.IsValidTarget(SpellManager.Spells[SpellSlot.Q].Range +
                                     SpellManager.Spells[SpellSlot.W].getTrueRange(target)))
            {
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("AA");
                return;
            }
            if (Program.ComboMenu["combo.useQ"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && Orbwalker.CanAutoAttack)
            {
                if (SpellManager.Spells[SpellSlot.Q].IsInRange(target))
                {
                    for (var i = 0; i < 3 - SpellEvents.QCount; i++)
                    {
                        Queuer.Queue.Add("AA");
                        Queuer.Queue.Add("Q");
                    }
                    Queuer.Queue.Add("AA");
                    Program.FastQ = true;
                    return;
                }
            }
            if (SpellManager.Spells[SpellSlot.W].IsReady() && SpellManager.Spells[SpellSlot.W].inRange(target))
            {
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("H");
                return;
            }
            if (Program.ComboMenu["combo.useW"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useE"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.E].IsReady() && SpellManager.Spells[SpellSlot.W].IsReady() &&
                target.IsValidTarget(SpellManager.Spells[SpellSlot.W].getTrueRange(target) +
                                     SpellManager.Spells[SpellSlot.E].Range))
            {
                Queuer.Queue.Add("E");
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("H");
                return;
            }
            if (Program.ComboMenu["combo.useE"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.E].IsReady() &&
                !target.IsValidTarget(Player.Instance.GetAutoAttackRange(target)))
            {
                Queuer.Queue.Add("E");
            }
            else
            {
                Queuer.Queue = new List<string>();
            }
        }

        public static void Harass()
        {
            var target = TargetSelector2.GetTarget(500, DamageType.Physical);

            if (target == null)
            {
                Target = null;
                Queuer.Queue = new List<string>();
                return;
            }
            if (Target == null || target.NetworkId != Target.NetworkId)
            {
                Target = target;
                Queuer.Queue = new List<string>();
            }
            if (Queuer.Queue.Any())
            {
                Queuer.DoQueue(target);
                return;
            }

            if (Program.HarassMenu["harass.useQ"].Cast<CheckBox>().CurrentValue &&
                Program.HarassMenu["harass.useW"].Cast<CheckBox>().CurrentValue &&
                Program.HarassMenu["harass.useE"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellManager.Spells[SpellSlot.E].IsReady() &&
                SpellManager.Spells[SpellSlot.W].IsReady() && (Queuer.tiamat == null || !Queuer.tiamat.CanUseItem())
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
                                     SpellManager.Spells[SpellSlot.W].getTrueRange(target)))
            {
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("AA");
                return;
            }
            if (Program.HarassMenu["harass.useQ"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && Orbwalker.CanAutoAttack)
            {
                if (SpellManager.Spells[SpellSlot.Q].IsInRange(target))
                {
                    for (var i = 0; i < 3 - SpellEvents.QCount; i++)
                    {
                        Queuer.Queue.Add("AA");
                        Queuer.Queue.Add("Q");
                    }
                    Queuer.Queue.Add("AA");
                    Program.FastQ = true;
                    return;
                }
            }
            if (Program.HarassMenu["harass.useW"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.W].IsReady() && SpellManager.Spells[SpellSlot.W].inRange(target))
            {
                Queuer.Queue.Add("W");
                return;
            }
            if (Program.HarassMenu["harass.useE"].Cast<CheckBox>().CurrentValue &&
                Program.HarassMenu["harass.useW"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.E].IsReady() && SpellManager.Spells[SpellSlot.W].IsReady() &&
                target.IsValidTarget(SpellManager.Spells[SpellSlot.W].getTrueRange(target) +
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
                Queuer.Queue = new List<string>();
            }
        }

        public static void WaveClear()
        {
            var target = EntityManager.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position.To2D(),
                SpellManager.Spells[SpellSlot.Q].Range).OrderBy(a => a.Health).FirstOrDefault();

            if (target == null)
            {
                Target = target;
                Queuer.Queue = new List<string>();
                return;
            }
            if (Target == null || target.NetworkId != Target.NetworkId)
            {
                Target = null;
                Queuer.Queue = new List<string>();
            }
            if (Queuer.Queue.Any())
            {
                Queuer.DoQueue(target);
                return;
            }

            if (Program.MinionClear["waveclear.useQ"].Cast<CheckBox>().CurrentValue &&
                Player.Instance.GetSpellDamage(target, SpellSlot.Q) > target.Health &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellManager.Spells[SpellSlot.Q].IsInRange(target))
            {
                Queuer.Queue.Add("Q");
                return;
            }

            if (Program.MinionClear["waveclear.useQ"].Cast<CheckBox>().CurrentValue &&
                Player.Instance.GetSpellDamage(target, SpellSlot.Q) + Player.Instance.GetAutoAttackDamage(target) >
                target.Health && SpellManager.Spells[SpellSlot.Q].IsReady() &&
                SpellManager.Spells[SpellSlot.Q].IsInRange(target) && Orbwalker.CanAutoAttack)
            {
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                return;
            }

            if (Program.MinionClear["waveclear.useW"].Cast<CheckBox>().CurrentValue &&
                Player.Instance.GetSpellDamage(target, SpellSlot.W) > target.Health &&
                SpellManager.Spells[SpellSlot.W].IsReady() && SpellManager.Spells[SpellSlot.W].IsInRange(target))
            {
                Queuer.Queue.Add("W");
                return;
            }
            if (Queuer.tiamat != null && Queuer.tiamat.CanUseItem() && target.IsValidTarget(300) &&
                Player.Instance.GetItemDamage(target, ItemId.Ravenous_Hydra_Melee_Only) > target.Health)
            {
                Queuer.tiamat.Cast();
            }
        }

        public static void LastHit()
        {
            var target = EntityManager.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position.To2D(),
                SpellManager.Spells[SpellSlot.Q].Range).OrderBy(a => a.Health).FirstOrDefault();

            if (target == null)
            {
                Target = null;
                Queuer.Queue = new List<string>();
                return;
            }
            if (Target == null || target.NetworkId != Target.NetworkId)
            {
                Target = target;
                Queuer.Queue = new List<string>();
            }
            if (Queuer.Queue.Any())
            {
                Queuer.DoQueue(target);
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
                SpellManager.Spells[SpellSlot.W].IsReady() && SpellManager.Spells[SpellSlot.W].IsInRange(target))
            {
                Queuer.Queue.Add("W");
            }
        }

        public static void Jungle()
        {
            var target = EntityManager.GetJungleMonsters(Player.Instance.Position.To2D(),
                SpellManager.Spells[SpellSlot.Q].Range + 300).OrderByDescending(a => a.MaxHealth).FirstOrDefault();

            if (target == null)
            {
                Target = null;
                Queuer.Queue = new List<string>();
                return;
            }
            if (Target == null || target.NetworkId != Target.NetworkId)
            {
                Target = target;
                Queuer.Queue = new List<string>();
            }
            if (Queuer.Queue.Any())
            {
                Queuer.DoQueue(target);
                return;
            }

            if (Program.Jungle["jungle.useQ"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && SpellManager.Spells[SpellSlot.Q].IsInRange(target) && Orbwalker.CanAutoAttack)
            {
                for (var i = 0; i < 3 - SpellEvents.QCount; i++)
                {
                    Queuer.Queue.Add("AA");
                    Queuer.Queue.Add("Q");
                }
                Queuer.Queue.Add("AA");
                Program.FastQ = true;
                return;
            }

            if (Program.Jungle["jungle.useW"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.W].IsReady() && SpellManager.Spells[SpellSlot.W].IsInRange(target))
            {
                Queuer.Queue.Add("W");
                return;
            }

            if (Program.Jungle["jungle.useE"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.E].IsReady())
            {
                Queuer.Queue.Add("E");
                return;
            }

            if (Queuer.tiamat != null && target.IsValidTarget(300) && Queuer.tiamat.CanUseItem())
            {
                Queuer.tiamat.Cast();
            }
        }
    }
}