using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace RivenBuddy
{
    public class TargetSelector2
    {
        private static AIHeroClient _target;
        private static int _lastClick;

        public static void Init()
        {
            Game.OnWndProc += Game_OnWndProc;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (_target != null)
            {
                Circle.Draw(Color.Cyan, 150, _target.Position);
            }
        }

        public static AIHeroClient GetTarget(float range, DamageType type, Vector2 secondaryPos = new Vector2())
        {
            if (_target == null || _target.IsDead || _target.Health <= 0 || !_target.IsValidTarget())
                _target = null;
            if (secondaryPos.IsValid() && _target.Distance(secondaryPos) < range || _target.IsValidTarget(range))
            {
                return _target;
            }
            return TargetSelector.GetTarget(range, type);
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg != 0x202) return;
            if (_lastClick + 500 <= Environment.TickCount)
            {
                _target =
                    ObjectManager.Get<AIHeroClient>()
                        .OrderBy(a => a.Distance(ObjectManager.Player))
                        .FirstOrDefault(a => a.IsEnemy && a.Distance(Game.CursorPos) < 200);
                if (_target != null)
                {
                    _lastClick = Environment.TickCount;
                }
            }
        }
    }
}