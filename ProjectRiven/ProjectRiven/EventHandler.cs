using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Menu.Values;

namespace ProjectRiven
{
    class EventHandler
    {
        public static void Init()
        {
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
            Obj_AI_Base.OnPlayAnimation += Obj_AI_Base_OnPlayAnimation;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;
            if (args.SData.Name.ToLower().Contains("itemtiamatcleave") && Riven.W.IsReady())
            {
                var target = TargetSelector.GetTarget(Riven.W.Range, DamageType.Physical);
                if (target != null)
                {
                    Core.DelayAction(() => Player.CastSpell(SpellSlot.W), 100);
                }
            }
        }

        private static void Obj_AI_Base_OnPlayAnimation(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe) return;
            var t = 0;
            switch (args.Animation)
            {
                case "Spell1a":
                    t = 291;
                    break;
                case "Spell1b":
                    t = 291;
                    break;
                case "Spell1c":
                    t = 393;
                    break;
                case "Spell2":
                    t = 170;
                    break;
                case "Spell3":
                    break;
                case "Spell4a":
                    t = 0;
                    break;
                case "Spell4b":
                    t = 150;
                    break;
            }
            if (t != 0 && (Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None))
            {
                Core.DelayAction(CancelAnimation, t - Game.Ping);
            }
        }

        private static void CancelAnimation()
        {
            Player.DoEmote(Emote.Dance);
            Orbwalker.ResetAutoAttack();
        }

        private static void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;
            var target = args.Target as Obj_AI_Base;
            if (args.SData.Name.ToLower().Contains("itemtiamatcleave") )
            {
                Orbwalker.ResetAutoAttack();
                if (Riven.W.IsReady())
                {
                    var target2 = TargetSelector.GetTarget(Riven.W.Range, DamageType.Physical);
                    if (target2 != null || Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None)
                    {
                        Player.CastSpell(SpellSlot.W);
                    }
                }
                return;
            }
            if (args.SData.Name.ToLower().Contains(Riven.W.Name.ToLower()))
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    if (Player.Instance.HasBuff("RivenFengShuiEngine") && Riven.R.IsReady())
                    {
                        var target2 = TargetSelector.GetTarget(Riven.R.Range, DamageType.Physical);
                        if (target2 != null && Player.Instance.CalculateDamageOnUnit(target2, DamageType.Physical, (float)(DamageHandler.RDamage(target2) + DamageHandler.WDamage())) > target2.Health)
                        {
                            Riven.R.Cast(target2);
                            return;
                        }
                    }
                    target = TargetSelector.GetTarget(Riven.W.Range, DamageType.Physical);
                    if (Riven.Q.IsReady() && target != null)
                    {
                        Player.CastSpell(SpellSlot.Q, target.Position);
                        return;
                    }
                }
                return;
            }
            if (args.SData.Name.ToLower().Contains(Riven.E.Name.ToLower()))
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    target = TargetSelector.GetTarget(Riven.W.Range, DamageType.Physical);
                    if (target != null && Player.Instance.Distance(target) < Riven.W.Range)
                    {
                        Player.CastSpell(SpellSlot.W);
                        return;
                    }
                }
            }
            if (args.SData.Name.ToLower().Contains(Riven.Q.Name.ToLower()))
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    if (Player.Instance.HasBuff("RivenFengShuiEngine") && Riven.R.IsReady())
                    {
                        var target2 = TargetSelector.GetTarget(Riven.R.Range, DamageType.Physical);
                        if (target2 != null && Player.Instance.CalculateDamageOnUnit(target2, DamageType.Physical, (float) (DamageHandler.RDamage(target2) + DamageHandler.QDamage())) > target2.Health)
                        {
                            Riven.R.Cast(target2);
                        }
                    }
                }
                return;
            }
            if (args.SData.IsAutoAttack() && target != null)
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
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

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) && target.IsJungleMinion())
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

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) && target.IsLaneMinion())
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

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && target.IsLaneMinion())
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

            }
        }
    }
}
