using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace RengarBuddy
{
    class StateManager
    {
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static void Combo()
        {
            var target = TargetSelector.GetTarget(1100, DamageType.Physical);
            switch ((int) _Player.Mana)
            {
                case 5:
                    if (Program.ComboMenu["ferocity"].Cast<CheckBox>().CurrentValue)
                    {
                        if (Program.ComboMenu["modeType"].Cast<CheckBox>().CurrentValue)
                        {
                            var healthPercent = Program.ComboMenu["wHealthPercentSlider"].Cast<Slider>().CurrentValue;
                            if (Program.W.IsReady() && _Player.HealthPercent <= healthPercent && Program.ComboMenu["wHealthPercent"].Cast<CheckBox>().CurrentValue)
                            {
                                Program.W.Cast();
                            }
                            else if (!Orbwalker.IsAutoAttacking && Program.ComboMenu["qInRange"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && target.IsValidTarget(_Player.GetAutoAttackRange(target)))
                            {
                                Program.Q.Cast();
                            }
                            else if (Program.E.IsReady() && Program.ComboMenu["eOutOfRange"].Cast<CheckBox>().CurrentValue)
                            {
                                Program.E.Cast(target);
                            }
                        }
                        else
                        {
                            var ferocityMode = Program.ComboMenu["selectedStackedSpell"].Cast<Slider>().CurrentValue;
                            switch (ferocityMode)
                            {
                                case 0:
                                    if (Orbwalker.IsAutoAttacking || target == null || !target.IsValidTarget(_Player.GetAutoAttackRange(target)) || !Program.Q.IsReady()) return;
                                    Program.Q.Cast();
                                    break;
                                case 1:
                                    if (!Program.W.IsReady()) return;
                                    Program.W.Cast();
                                    break;
                                case 2:
                                    if (target == null || !Program.E.IsReady()) return;
                                    Program.E.Cast(target);
                                    break;
                            }
                        }
                    }
                    break;
                default:
                    if (Program.W.IsReady() && target.IsValidTarget(Program.W.Range) && Program.ComboMenu["wCombo"].Cast<CheckBox>().CurrentValue)
                    {
                        Program.W.Cast();
                    }
                    if (Program.Q.IsReady() && Program.ComboMenu["qCombo"].Cast<CheckBox>().CurrentValue && !Orbwalker.IsAutoAttacking && target.IsValidTarget(_Player.GetAutoAttackRange(target)))
                    {
                        Program.Q.Cast();
                    }
                    if (Program.E.IsReady() && Program.ComboMenu["eCombo"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(Program.E.Range))
                    {
                        Program.E.Cast(target);
                    }
                    break;
            }
        }

        public void Harass()
        {
            var target = TargetSelector.GetTarget(1100, DamageType.Physical);
            switch ((int)_Player.Mana)
            {
                case 5:
                    if (Program.HarassMenu["save5StacksHarass"].Cast<CheckBox>().CurrentValue)
                        return;
                    var ferocityMode = Program.HarassMenu["selectedStackedSpellHarass"].Cast<Slider>().CurrentValue;
                    switch (ferocityMode)
                    {
                        case 0:
                            if (Orbwalker.IsAutoAttacking || target == null || !target.IsValidTarget(_Player.GetAutoAttackRange(target)) || !Program.Q.IsReady()) return;
                            Program.Q.Cast();
                            break;
                        case 1:
                            if (!Program.W.IsReady()) return;
                            Program.W.Cast();
                            break;
                        case 2:
                            if (target == null || !Program.E.IsReady()) return;
                            Program.E.Cast(target);
                            break;
                    }
                    break;
                default:
                    if (Program.W.IsReady() && target.IsValidTarget(Program.W.Range) && Program.HarassMenu["wHarass"].Cast<CheckBox>().CurrentValue)
                    {
                        Program.W.Cast();
                    }
                    if (Program.Q.IsReady() && Program.HarassMenu["qHarass"].Cast<CheckBox>().CurrentValue && !Orbwalker.IsAutoAttacking && target.IsValidTarget(_Player.GetAutoAttackRange(target)))
                    {
                        Program.Q.Cast();
                    }
                    if (Program.E.IsReady() && Program.HarassMenu["eHarass"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(Program.E.Range))
                    {
                        Program.E.Cast(target);
                    }
                    break;
            }
        }

        public void LastHit()
        {
            if (Orbwalker.IsAutoAttacking) return;
            Orbwalker.ForcedTarget = null;
            foreach (var source in ObjectManager.Get<Obj_AI_Minion>().Where(a => a.IsEnemy && a.Distance(_Player) < 1000))
            {
                if (Program.FarmMenu["qLastHit"].Cast<CheckBox>().CurrentValue && Damage.Q1(source) > source.Health && source.Distance(_Player) < _Player.GetAutoAttackRange(source))
                {
                    Program.Q.Cast();
                    Orbwalker.ForcedTarget = source;
                    Player.IssueOrder(GameObjectOrder.AttackUnit, source);
                    return;
                }
                if (Damage.W(source) > source.Health && source.Distance(_Player) < Program.W.Range)
                {
                    Program.W.Cast();
                    return;
                }
                if (Program.FarmMenu["eLastHit"].Cast<CheckBox>().CurrentValue && Damage.E(source) > source.Health && source.Distance(_Player) < Program.E.Range)
                {
                    Program.E.Cast(source);
                    return;
                }
            }
        }

        public void WaveClear()
        {
            if (Orbwalker.IsAutoAttacking) return;
            Orbwalker.ForcedTarget = null;
            foreach (var source in ObjectManager.Get<Obj_AI_Minion>().Where(a => a.IsEnemy && a.Distance(_Player) < 1000))
            {

                if (Program.FarmMenu["saveStacksWC"].Cast<CheckBox>().CurrentValue && (int) _Player.Mana == 5)
                    return;
                if((int) _Player.Mana == 5) { 
                    var ferocityMode = Program.FarmMenu["selectedStackedSpellWC"].Cast<Slider>().CurrentValue;
                    switch (ferocityMode)
                    {
                        case 0:
                            if (Orbwalker.IsAutoAttacking || !source.IsValidTarget(_Player.GetAutoAttackRange(source)) || !Program.Q.IsReady()) return;
                            Program.Q.Cast();
                            break;
                        case 1:
                            if (!Program.W.IsReady()) return;
                            Program.W.Cast();
                            break;
                        case 2:
                            if (source == null || !Program.E.IsReady()) return;
                            Program.E.Cast(source);
                            break;
                    }
                    return;
                }

                if (Program.FarmMenu["qWaveClear"].Cast<CheckBox>().CurrentValue && Damage.Q1(source) > source.Health && source.Distance(_Player) < _Player.GetAutoAttackRange(source))
                {
                    Program.Q.Cast();
                    Orbwalker.ForcedTarget = source;
                    Player.IssueOrder(GameObjectOrder.AttackUnit, source);
                    return;
                }
                if (Program.FarmMenu["wWaveClear"].Cast<CheckBox>().CurrentValue && Damage.W(source) > source.Health && source.Distance(_Player) < Program.W.Range)
                {
                    Program.W.Cast();
                    return;
                }
                if (Program.FarmMenu["eWaveClear"].Cast<CheckBox>().CurrentValue && Damage.E(source) > source.Health && source.Distance(_Player) < Program.E.Range)
                {
                    Program.E.Cast(source);
                    return;
                }
            }
        }

        public void Jungle()
        {
            if (Orbwalker.IsAutoAttacking) return;
            Orbwalker.ForcedTarget = null;
            var source =
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(a => a.Team == GameObjectTeam.Neutral && a.Distance(_Player) < 1000)
                    .OrderByDescending(a => a.MaxHealth)
                    .FirstOrDefault();

            if (Program.JungleMenu["saveStacksJungle"].Cast<CheckBox>().CurrentValue && (int)_Player.Mana == 5)
                return;
            if ((int) _Player.Mana == 5)
            {
                var ferocityMode = Program.JungleMenu["selectedStackedSpellJNG"].Cast<Slider>().CurrentValue;
                switch (ferocityMode)
                {
                    case 0:
                        if (Orbwalker.IsAutoAttacking || !source.IsValidTarget(_Player.GetAutoAttackRange(source)) ||
                            !Program.Q.IsReady()) return;
                        Program.Q.Cast();
                        break;
                    case 1:
                        if (!Program.W.IsReady()) return;
                        Program.W.Cast();
                        break;
                    case 2:
                        if (source == null || !Program.E.IsReady()) return;
                        Program.E.Cast(source);
                        break;
                }
                return;
            }

            if (Program.FarmMenu["qJng"].Cast<CheckBox>().CurrentValue && source.Distance(_Player) < _Player.GetAutoAttackRange(source))
                {
                    Program.Q.Cast();
                    Orbwalker.ForcedTarget = source;
                    Player.IssueOrder(GameObjectOrder.AttackUnit, source);
                    return;
                }
                if (Program.FarmMenu["wJng"].Cast<CheckBox>().CurrentValue && source.Distance(_Player) < Program.W.Range)
                {
                    Program.W.Cast();
                    return;
                }
                if (Program.FarmMenu["eJng"].Cast<CheckBox>().CurrentValue && source.Distance(_Player) < Program.E.Range)
                {
                    Program.E.Cast(source);
                }
        }
    }
}
