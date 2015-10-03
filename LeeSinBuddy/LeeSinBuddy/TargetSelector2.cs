using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace LeeSinBuddy
{
    public static class TargetSelector2
    {
        public static AIHeroClient ForcedTarget;
        private static int _lastClick;
        public static bool IsSelected
        {
            get { return ForcedTarget != null; }
        }

        public static void init()
        {
            Game.OnWndProc += Game_OnWndProc;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (ForcedTarget != null)
            {
                new Circle
                {
                    Color = Color.Red,
                    Radius = 150
                }.Draw(ForcedTarget.Position);
            }
        }

        public static AIHeroClient GetTarget(int range, DamageType type, Vector2 secondaryPos = new Vector2())
        {
            if (ForcedTarget == null || ForcedTarget.IsDead || ForcedTarget.Health <= 0 || !ForcedTarget.IsValidTarget())
                ForcedTarget = null;
            if (secondaryPos.IsValid() && ForcedTarget.Distance(secondaryPos) < range ||
                ForcedTarget.IsValidTarget(range))
            {
                return ForcedTarget;
            }
            return TargetSelector.GetTarget(range, type);
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg != 0x202) return;
            if (_lastClick + 500 <= Environment.TickCount)
            {
                ForcedTarget =
                    ObjectManager.Get<AIHeroClient>()
                        .OrderBy(a => a.Distance(ObjectManager.Player))
                        .FirstOrDefault(a => a.IsEnemy && a.Distance(Game.CursorPos) < 200);
                if (ForcedTarget != null)
                {
                    _lastClick = Environment.TickCount;
                }
            }
        }
    }
}