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

        public static int LastCastW;

        public static void Init()
        {
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
            Obj_AI_Base.OnPlayAnimation += Obj_AI_Base_OnPlayAnimation;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.SData.Name.ToLower().Contains(Riven.E.Name.ToLower()))
            {
                LastCastW = Environment.TickCount;
                return;
            }
            if (args.SData.Name.ToLower().Contains(Riven.Q.Name.ToLower()))
            {
                Core.DelayAction(() =>
                {
                    if (!Player.Instance.IsRecalling())
                    {
                        Player.CastSpell(SpellSlot.Q,
                            Orbwalker.LastTarget != null && Orbwalker.LastAutoAttack - Environment.TickCount < 3000
                                ? Orbwalker.LastTarget.Position
                                : Game.CursorPos);
                    }
                }, 3480);
                return;
            }
            if (args.SData.Name.ToLower().Contains(Riven.R.Name.ToLower()))
            {
                Core.DelayAction(() =>
                {
                    if (Player.Instance.IsRecalling() || !Riven.R.IsReady() || !Player.Instance.HasBuff("RivenFengShuiEngine")) return;
                    foreach (var enemy in EntityManager.Heroes.Enemies.Where(enemy => enemy.IsValidTarget(Riven.R.Range)).Where(enemy => Riven.R.Cast(enemy)))
                    {
                        break;
                    }
                }, 14800);
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
            if (!sender.IsMe || Riven.BurstActive && !BurstHandler.ComboFinished) return;
            var target = args.Target as Obj_AI_Base;

            // Hydra
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

            //W
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
                }

                target = (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) ? TargetSelector.GetTarget(Riven.E.Range + Riven.W.Range, DamageType.Physical) : (Obj_AI_Base)Orbwalker.LastTarget;
                if (Riven.Q.IsReady() && Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None && target != null)
                {
                    Player.CastSpell(SpellSlot.Q, target.Position);
                    return;
                }
                return;
            }

            //E
            if (args.SData.Name.ToLower().Contains(Riven.E.Name.ToLower()))
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
                    if (Riven.IsRActive && Riven.R.IsReady() && !Player.Instance.HasBuff("RivenFengShuiEngine"))
                    {
                        Player.CastSpell(SpellSlot.R);
                    }
                    target = TargetSelector.GetTarget(Riven.W.Range, DamageType.Physical);
                    if (target != null && Player.Instance.Distance(target) < Riven.W.Range)
                    {
                        Player.CastSpell(SpellSlot.W);
                        return;
                    }
                }
            }

            //Q
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
                    StateHandler.ComboAfterAa(target);
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) && target.IsJungleMinion())
                {
                    StateHandler.JungleAfterAa(target);
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) && target.IsLaneMinion())
                {
                    StateHandler.LastHitAfterAa(target);
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && target.IsLaneMinion())
                {
                    StateHandler.LaneClearAfterAa(target);
                }
            }
        }
    }
}
