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
        public static int PassiveStacks;
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
            Game.OnUpdate += delegate { UpdateSpells(); };
        }

        private static void Obj_AI_Base_OnPlayAnimation(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe || Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.None && !Program.ComboMenu["combo.alwaysCancelQ"].Cast<CheckBox>().CurrentValue)
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
            if (t != 0)
            {
                Core.DelayAction(CancelAnimation, t - Game.Ping);
            }
        }

        private static void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;
            if (args.SData.IsAutoAttack())
            {
                Queuer.Remove("AA");
                if (Queuer.Queue.Any() && Queuer.Queue[0] == "Q")
                {
                    var target = args.Target as Obj_AI_Base;
                    if (target != null && (SpellManager.Spells[SpellSlot.R].IsReady() &&
                    Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                    (int) DamageHandler.RDamage(target)) >= target.Health && SpellEvents.HasR2 &&
                    Program.ComboMenu["combo.useR2"].Cast<CheckBox>().CurrentValue))
                    {
                        Queuer.Queue = new List<string>();
                        return;
                    }
                    Player.CastSpell(SpellSlot.Q, args.Target.Position);
                    Queuer.Remove("Q");
                    Orbwalker.ResetAutoAttack();
                }
                else if (!Queuer.Queue.Any() && Queuer.tiamat != null && Queuer.tiamat.CanUseItem() && Program.ComboMenu["combo.hydra"].Cast<CheckBox>().CurrentValue && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && args.Target is AIHeroClient)
                {
                    Queuer.tiamat.Cast();
                }
            }
        }

        public static void CancelAnimation()
        {
            Player.DoEmote(Emote.Dance);
            var target = Orbwalker.LastTarget;
            if (target != null)
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
        }

        public static void UpdateSpells()
        {
            if (LastCast["Q"] + 3450 < Environment.TickCount && Program.ComboMenu["combo.keepQAlive"].Cast<CheckBox>().CurrentValue && QCount > 0 && !Player.Instance.IsRecalling())
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

            if (LastCast["Q"] + 1000 < Environment.TickCount && Orbwalker.DisableMovement)
                Orbwalker.DisableMovement = false;

            if ((Queuer.Queue.Any() && Queuer.Queue[0] == "Q" || Queuer.Queue.Count > 1 && Queuer.Queue[1] == "Q" && Orbwalker.CanAutoAttack) &&
                (Player.GetSpell(SpellSlot.Q).IsReady || Player.GetSpell(SpellSlot.Q).Cooldown <= 0.25) && QCount > 0 && States.Target != null && States.Target.IsValidTarget())
            {
                Orbwalker.ResetAutoAttack();
                Orbwalker.DisableMovement = true;
            }
            else
            {
                Orbwalker.DisableMovement = false;
            }

            if (HasR && LastCast["R1"] + 15000 < Environment.TickCount)
            {
                HasR = false; // Reset R
                HasR2 = false; // Reset R2
            }
            if (PassiveStacks != 0 && LastCast["PAS"] + 4000 < Environment.TickCount) PassiveStacks = 0; // Reset Passive
            if (LastCast["Q"] + 3470 < Environment.TickCount) QCount = 0; // Reset Passive
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;
            if (args.SData.IsAutoAttack())
            {
                if (PassiveStacks > 0) PassiveStacks--;
                LastCast["AA"] = Environment.TickCount;
            }
            switch (args.SData.Name.ToLower())
            {
                case "itemtiamatcleave":
                    Queuer.Remove("H");
                    break;

                case "riventricleave": //Q

                    LastCast["Q"] = Environment.TickCount;
                    Queuer.Remove("Q");
                    if (PassiveStacks <= 2) PassiveStacks++;

                    // Managing Q Stacks
                    if (QCount <= 1) QCount++;
                    else QCount = 0;
                    break;


                case "rivenmartyr": //W
                    LastCast["W"] = Environment.TickCount;
                    if (PassiveStacks <= 2) PassiveStacks++;
                    Queuer.Remove("W");
                    break;


                case "rivenfeint": //E
                    LastCast["E"] = Environment.TickCount;
                    if (PassiveStacks <= 2) PassiveStacks++;
                    Queuer.Remove("E");
                    break;


                case "rivenfengshuiengine": //R1
                    LastCast["R1"] = Environment.TickCount;
                    if (PassiveStacks <= 2) PassiveStacks++;
                    Queuer.Remove("R1");
                    HasR = true;
                    HasR2 = true;
                    break;


                case "rivenizunablade": //R2
                    LastCast["R2"] = Environment.TickCount;
                    if (PassiveStacks <= 2) PassiveStacks++;
                    Queuer.Remove("R2");
                    HasR2 = false;
                    break;
            }
        }
    }
}