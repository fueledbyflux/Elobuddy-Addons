using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace YasuoBuddy
{
    public class TargetSelector2
    {
        public static AIHeroClient Target;
        private static int _lastClick;

        public static bool IsSelected
        {
            get { return Target != null; }
        }

        public static void Init()
        {
            Game.OnWndProc += Game_OnWndProc;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Target != null)
            {
                Circle.Draw(Color.Red, 150, Target.Position);
            }
        }

        public static AIHeroClient GetTarget(float range, DamageType type, Vector2 secondaryPos = new Vector2())
        {
            if (Target == null || Target.IsDead || Target.Health <= 0 || !Target.IsValidTarget())
                Target = null;
            if (secondaryPos.IsValid() && Target.Distance(secondaryPos) < range || Target.IsValidTarget(range))
            {
                return Target;
            }
            return TargetSelector.GetTarget(range, type);
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg != 0x202) return;
            if (_lastClick + 500 <= Environment.TickCount)
            {
                Target =
                    ObjectManager.Get<AIHeroClient>()
                        .OrderBy(a => a.Distance(ObjectManager.Player))
                        .FirstOrDefault(a => a.IsEnemy && a.Distance(Game.CursorPos) < 200);
                if (Target != null)
                {
                    _lastClick = Environment.TickCount;
                }
            }
        }
    }
}