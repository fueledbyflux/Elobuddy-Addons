using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace RengarBuddy
{
    class MeleeOrbwalker
    {
        public static void Init()
        {
            Orbwalker.DisableAttacking = true;
            Orbwalker.DisableMovement = true;

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
                Orbwalk();
            }   
            else
            {
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;

            }
        }

        private static long _lastAaTick;

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                _lastAaTick = Environment.TickCount;
            }
        }

        public static bool CanAttack()
        {
            if (_lastAaTick <= Environment.TickCount)
                return Environment.TickCount + Game.Ping / 2 + 25 >= _lastAaTick + ObjectManager.Player.AttackDelay * 1000;
            return false;
        }

        public static bool CanMove()
        {
            if (_lastAaTick <= Environment.TickCount)
                return Environment.TickCount + Game.Ping / 2 >= _lastAaTick + ObjectManager.Player.AttackCastDelay * 1000 + 25;
            return false;
        }

        public static void Orbwalk()
        {
            if (CanAttack())
            {
                var target = TargetSelector2.GetTarget(ObjectManager.Player.GetAutoAttackRange(), DamageType.Physical);
                if (target != null)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                    _lastAaTick = Environment.TickCount;
                    return;
                }
            }
            if (CanMove())
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                return;
            }
        }
    }
}
