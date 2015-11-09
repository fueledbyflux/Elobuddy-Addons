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

        public static void Burst()
        {
            var target = TargetSelector2.GetTarget(SpellManager.Spells[SpellSlot.E].Range + SpellManager.Spells[SpellSlot.W].Range + 400, DamageType.Physical);

            Orbwalker.ForcedTarget = target;
            Orbwalker.OrbwalkTo(Game.CursorPos);

            var flash = Player.Spells.FirstOrDefault(a => a.SData.Name == "summonerflash");

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
            if (Program.ComboMenu["burst.flash"].Cast<CheckBox>().CurrentValue && SpellManager.Spells[SpellSlot.Q].IsReady() &&
                SpellManager.Spells[SpellSlot.W].IsReady() &&
                SpellManager.Spells[SpellSlot.E].IsReady() &&
                SpellManager.Spells[SpellSlot.R].IsReady() &&
                !SpellEvents.HasR && flash != null && flash.IsReady)
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
            if (target.IsValidTarget(SpellManager.Spells[SpellSlot.E].Range + SpellManager.Spells[SpellSlot.W].Range) && (!Program.ComboMenu["burst.flash"].Cast<CheckBox>().CurrentValue || flash == null || !flash.IsReady) &&
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
                if(state) Queuer.DoQueue(target);
                return;
            }

            float THealth = state ? target.Health : target.Health - Player.Instance.GetAutoAttackDamage(target, true);

            var comboDmg = DamageHandler.ComboDamage(target, true);
            if (Program.ComboMenu["combo.useQ"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useW"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useE"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useR"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useR2"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() &&
                SpellManager.Spells[SpellSlot.W].IsReady() &&
                SpellManager.Spells[SpellSlot.E].IsReady() &&
                SpellManager.Spells[SpellSlot.R].IsReady() &&
                SpellEvents.QCount == 0 &&
                !SpellEvents.HasR2
                &&
                (Program.IsRActive ||
                 target.IsValidTarget(SpellManager.Spells[SpellSlot.E].Range + SpellManager.Spells[SpellSlot.W].Range)
                 && comboDmg < THealth &&
                 comboDmg +
                 Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                     (int) DamageHandler.RDamage(target, comboDmg)) >= THealth))
            {
                Queuer.Queue.Add("E");
                Queuer.Queue.Add("R1");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                return;
            }

            if (SpellManager.Spells[SpellSlot.R].IsReady() &&
                Program.ComboMenu["combo.useR"].Cast<CheckBox>().CurrentValue &&
                (Program.IsRActive ||
                 comboDmg < THealth &&
                 comboDmg + Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                     (int) DamageHandler.RDamage(target, comboDmg)) >= THealth) && !SpellEvents.HasR)
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
                Program.ComboMenu["combo.useR2"].Cast<CheckBox>().CurrentValue)
            {
                if (Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                    (int) DamageHandler.RDamage(target)) >= THealth)
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
                    (int) (DamageHandler.RDamage(target) + DamageHandler.QDamage())) >= THealth)
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
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("Q");
                Queuer.Queue.Add("AA");
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
                Queuer.Queue.Add("H");
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
                Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                Queuer.Queue.Add("AA");
                return;
            }
            if (SpellManager.Spells[SpellSlot.W].IsReady() && SpellManager.Spells[SpellSlot.W].inRange(target))
            {
                if (Program.ComboMenu["combo.hydra"].Cast<CheckBox>().CurrentValue) Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
                return;
            }

            if (Program.ComboMenu["combo.useQ"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.Q].IsReady() && Orbwalker.CanAutoAttack)
            {
                if (SpellManager.Spells[SpellSlot.Q].IsInRange(target))
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
            if (Program.ComboMenu["combo.useW"].Cast<CheckBox>().CurrentValue &&
                Program.ComboMenu["combo.useE"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.E].IsReady() && SpellManager.Spells[SpellSlot.W].IsReady() &&
                target.IsValidTarget(SpellManager.Spells[SpellSlot.W].getTrueRange(target) +
                                     SpellManager.Spells[SpellSlot.E].Range))
            {
                Queuer.Queue.Add("E");
                if (Program.ComboMenu["combo.hydra"].Cast<CheckBox>().CurrentValue) Queuer.Queue.Add("H");
                Queuer.Queue.Add("W");
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

        public static void Harass(bool state = true)
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
                if (state) Queuer.DoQueue(target);
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
                SpellManager.Spells[SpellSlot.W].IsReady() && SpellManager.Spells[SpellSlot.W].inRange(target))
            {
                if(Program.HarassMenu["harass.hydra"].Cast<CheckBox>().CurrentValue) Queuer.Queue.Add("H");
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

        public static void WaveClear(bool state = true)
        {
            var target = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position,
                SpellManager.Spells[SpellSlot.Q].Range + 100).OrderBy(a => a.Health).FirstOrDefault();

            if (target == null)
            {
                Target = null;
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

            if (Queuer.tiamat != null && Queuer.tiamat.CanUseItem() && target.IsValidTarget(300) && Program.MinionClear["waveclear.hydra"].Cast<CheckBox>().CurrentValue)
            {
                Queuer.tiamat.Cast();
            }
        }

        public static void LastHit(bool state = true)
        {
            var target = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position,
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
                if (state) Queuer.DoQueue(target);
                return;
            }


            if (Program.Jungle["jungle.useE"].Cast<CheckBox>().CurrentValue &&
                SpellManager.Spells[SpellSlot.E].IsReady())
            {
                Queuer.Queue.Add("AA");
                Queuer.Queue.Add("E");
                if (Queuer.tiamat != null && Queuer.tiamat.CanUseItem())
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
            if (SpellEvents.LastCast["Q"] + 600 < Environment.TickCount && SpellEvents.LastCast["Q"] + 1400 > Environment.TickCount && Player.GetSpell(SpellSlot.E).State == SpellState.Ready)
            {
                Player.CastSpell(SpellSlot.E, Game.CursorPos);
                SpellEvents.LastCast["Q"] = Environment.TickCount;
            }
            if (SpellEvents.LastCast["E"] + 600 < Environment.TickCount && SpellEvents.LastCast["E"] + 600 < Environment.TickCount && Player.GetSpell(SpellSlot.Q).State == SpellState.Ready)
            {
                Player.CastSpell(SpellSlot.Q, Game.CursorPos);
                SpellEvents.LastCast["Q"] = Environment.TickCount;
            }
            
        }
    }
}