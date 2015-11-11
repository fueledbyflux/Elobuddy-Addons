using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;

namespace RivenBuddy
{
    internal class SpellEvents
    {
        public static Dictionary<string, long> LastCast = new Dictionary<string, long>();
        public static int QCount;
        public static bool HasR;
        public static bool HasR2;

        public static void Init()
        {
            LastCast.Add("Q", 0);
            LastCast.Add("W", 0);
            LastCast.Add("E", 0);
            LastCast.Add("R1", 0);
            LastCast.Add("R2", 0);
            LastCast.Add("PAS", 0);
            LastCast.Add("AA", 0);

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Obj_AI_Base.OnPlayAnimation += Obj_AI_Base_OnPlayAnimation;
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
        }

        private static void Obj_AI_Base_OnPlayAnimation(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }
            var t = 0;
            switch (args.Animation)
            {
                case "Spell1a":
                    t = 291;
                    QCount = 1;
                    break;
                case "Spell1b":
                    t = 291;
                    QCount = 2;
                    break;
                case "Spell1c":
                    t = 393;
                    QCount = 0;
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
            if (t != 0 && (Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None || Program.ComboMenu["combo.alwaysCancelQ"].Cast<CheckBox>().CurrentValue))
            {
                Core.DelayAction(CancelAnimation, t - Game.Ping);
            }
        }

        private static void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;
            if (args.SData.Name.ToLower().Contains("itemtiamatcleave"))
            {
                Queuer.Remove("H");
                Orbwalker.ResetAutoAttack();
            }
            if (args.SData.Name.ToLower().Contains("riventricleave"))
            {
                Orbwalker.ResetAutoAttack();
            }
            if (args.SData.IsAutoAttack())
            {
                Queuer.Remove("AA");
                if (Program.HumanizerMenu["humanizerQSlow"].Cast<Slider>().CurrentValue == 0)
                {
                    if (Queuer.Queue.Any() && Queuer.Queue[0] == "Q")
                    {
                        var target = args.Target as Obj_AI_Base;
                        if (target != null && (SpellManager.Spells[SpellSlot.R].IsReady() &&
                                               Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                                                   (int)DamageHandler.RDamage(target)) >= target.Health && HasR2 &&
                                               Program.ComboMenu["combo.useR2"].Cast<CheckBox>().CurrentValue &&
                                               target.Health > Player.Instance.GetAutoAttackDamage(target, true)))
                        {
                            Program.R2.Cast(target);
                            Queuer.Queue = new List<string>();
                            return;
                        }
                        Player.CastSpell(SpellSlot.Q, args.Target.Position);
                        Queuer.Remove("Q");
                        if(QCount < 2) Orbwalker.ResetAutoAttack();
                    }
                    else if (!Queuer.Queue.Any() && Queuer.Tiamat != null && Queuer.Tiamat.CanUseItem() &&
                             Program.ComboMenu["combo.hydra"].Cast<CheckBox>().CurrentValue &&
                             Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                             args.Target is AIHeroClient)
                    {
                        Queuer.Tiamat.Cast();
                        Orbwalker.ResetAutoAttack();
                        ReQueue();
                    }
                    return;
                }
                Core.DelayAction(() =>
                {
                    if (Queuer.Queue.Any() && Queuer.Queue[0] == "Q")
                    {
                        var target = args.Target as Obj_AI_Base;
                        if (target != null && (SpellManager.Spells[SpellSlot.R].IsReady() &&
                                               Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                                                   (int) DamageHandler.RDamage(target)) >= target.Health && HasR2 &&
                                               Program.ComboMenu["combo.useR2"].Cast<CheckBox>().CurrentValue &&
                                               target.Health > Player.Instance.GetAutoAttackDamage(target, true)))
                        {
                            Program.R2.Cast(target);
                            Queuer.Queue = new List<string>();
                            return;
                        }
                        Player.CastSpell(SpellSlot.Q, args.Target.Position);
                        Queuer.Remove("Q");
                        Orbwalker.ResetAutoAttack();
                        ReQueue();
                    }
                    else if (!Queuer.Queue.Any() && Queuer.Tiamat != null && Queuer.Tiamat.CanUseItem() &&
                             Program.ComboMenu["combo.hydra"].Cast<CheckBox>().CurrentValue &&
                             Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                             args.Target is AIHeroClient)
                    {
                        Queuer.Tiamat.Cast();
                        Orbwalker.ResetAutoAttack();
                    }
                }, Program.HumanizerMenu["humanizerQSlow"].Cast<Slider>().CurrentValue);

            }
        }

        public static void CancelAnimation()
        {
            Player.DoEmote(Emote.Dance);
            Orbwalker.ResetAutoAttack();
        }

        public static void UpdateSpells()
        {
            if (LastCast["Q"] + 3480 < Environment.TickCount && Program.ComboMenu["combo.keepQAlive"].Cast<CheckBox>().CurrentValue && QCount > 0 && !Player.Instance.IsRecalling())
            {
                Player.CastSpell(SpellSlot.Q, Game.CursorPos);
                Core.DelayAction(Program.IssueLastOrder, 291);
            }

            if (HasR2 && LastCast["R1"] + 14800 < Environment.TickCount && Program.ComboMenu["combo.useRBeforeExpire"].Cast<CheckBox>().CurrentValue && !Player.Instance.IsRecalling())
            {
                foreach (var target in EntityManager.Heroes.Enemies.Where(a => a.Distance(Player.Instance) < 1000))
                {
                    Program.R2.Cast(target);
                }
            }

            if (HasR && LastCast["R1"] + 15000 < Environment.TickCount)
            {
                HasR = false; // Reset R
                HasR2 = false; // Reset R2
            }
            if (LastCast["Q"] + 3500 < Environment.TickCount) QCount = 0; // Reset Passive
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;
            switch (args.SData.Name.ToLower())
            {
                case "riventricleave": //Q
                    LastCast["Q"] = Environment.TickCount;
                    Queuer.Remove("Q");
                    break;
                    
                case "rivenmartyr": //W
                    LastCast["W"] = Environment.TickCount;
                    break;


                case "rivenfeint": //E
                    LastCast["E"] = Environment.TickCount;
                    break;


                case "rivenfengshuiengine": //R1
                    LastCast["R1"] = Environment.TickCount;
                    HasR = true;
                    HasR2 = true;
                    break;


                case "rivenizunablade": //R2
                    LastCast["R2"] = Environment.TickCount;
                    HasR2 = false;
                    break;
            }
        }
        private static void ReQueue()
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                States.Combo(false);
                return;
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                States.Harass(false);
                return;
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                States.Jungle(false);
                return;
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                var target = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position,
                    SpellManager.Spells[SpellSlot.Q].Range + 300).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) && target == null)
                {
                    States.WaveClear(false);
                    return;
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                States.LastHit(false);
                return;
            }
        }

    }
}