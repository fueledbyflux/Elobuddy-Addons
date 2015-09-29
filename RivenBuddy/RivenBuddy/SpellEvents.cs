using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;

namespace RivenBuddy
{
    internal class SpellEvents
    {
        public static Dictionary<string, long> LastCast = new Dictionary<string, long>();
        public static int QCount;
        public static int PassiveStacks;
        public static bool HasR;
        public static bool HasR2;
        public static bool Cancel;
        public static bool FastQ;
        public static bool CanCastAnimation;

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
            if (!sender.IsMe || Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.None)
            {
                return;
            }
            int t = 0;
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
                if ((Player.GetSpell(SpellSlot.Q).IsReady || Player.GetSpell(SpellSlot.Q).Cooldown <= 0.25) && FastQ)
                {
                    Player.CastSpell(SpellSlot.Q, args.Target.Position);
                    Orbwalker.ResetAutoAttack();
                    if (QCount == 0) QCount = 1;
                    if (QCount == 2 || QCount == 0)
                    {
                        FastQ = false;
                        Orbwalker.DisableMovement = false;
                    }
                    else
                    {
                        FastQ = true;
                        Orbwalker.DisableMovement = true;
                    }
                    return;
                }
                if (!FastQ && Queuer.Queue.Any() && Queuer.Queue[0] == "Q")
                {
                    Player.CastSpell(SpellSlot.Q, args.Target.Position);
                    Queuer.Remove("Q");
                    Orbwalker.ResetAutoAttack();
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
            if (LastCast["Q"] + 1000 < Environment.TickCount && Orbwalker.DisableMovement)
                Orbwalker.DisableMovement = false;

            if (FastQ)
            {
                Orbwalker.ResetAutoAttack();
                Orbwalker.DisableMovement = true;
                return;
            }

            if (HasR && LastCast["R1"] + 15000 < Environment.TickCount) {
                HasR = false; // Reset R
                HasR2 = false; // Reset R2
            }
            if (QCount != 0 && LastCast["Q"] + 5000 < Environment.TickCount) PassiveStacks = 0; // Reset Passive
            if (PassiveStacks != 0 && LastCast["PAS"] + 4000 < Environment.TickCount) QCount = 0; // Reset Passive
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
                    Queuer.Remove("R");
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