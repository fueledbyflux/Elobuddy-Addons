using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace VayneBuddy
{
    class WallQ
    {
        public static AIHeroClient _Player => ObjectManager.Player;
        

        public static void Drawing_OnDraw()
        {
            Vector2 drakeWallQPos = new Vector2(12050, 4827);
            Vector2 midWallQPos = new Vector2(6962, 8952);
            if (drakeWallQPos.Distance(_Player) < 3000)
                new Circle() { Color = _Player.Distance(drakeWallQPos) <= 100 ? Color.DodgerBlue : Color.White, Radius = 100 }.Draw(drakeWallQPos.To3D());
            if (midWallQPos.Distance(_Player) < 3000)
                new Circle() { Color = _Player.Distance(midWallQPos) <= 100 ? Color.DodgerBlue : Color.White,  Radius = 100 }.Draw(midWallQPos.To3D());

        }

        public static Dictionary<Action, int> DelayedActions = new Dictionary<Action, int>();

        public static void WallTumble()
        {
            foreach (var delayedAction in DelayedActions)
            {
                if (delayedAction.Value <= Environment.TickCount)
                {
                    delayedAction.Key.Invoke();
                    DelayedActions.Remove(delayedAction.Key);
                    return;
                }
            }
            if (!Program.Q.IsReady()) return;

            Vector2 drakeWallQPos = new Vector2(11514, 4462);
            Vector2 midWallQPos = new Vector2(6667, 8794);

            var selectedPos = drakeWallQPos.Distance(_Player) < midWallQPos.Distance(_Player) ? drakeWallQPos :  midWallQPos;
            var walkPos = drakeWallQPos.Distance(_Player) < midWallQPos.Distance(_Player)
                ? new Vector2(12050, 4827)
                : new Vector2(6962, 8952);
            if (_Player.Distance(walkPos) < 200 && _Player.Distance(walkPos) > 1)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, walkPos.To3D());
            }
            else if(_Player.Distance(walkPos) <= 10)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, walkPos.To3D());
                DelayedActions.Add(delegate {Program.Q.Cast(selectedPos.To3D());}, Environment.TickCount + 106 + (Game.Ping/2));
            }
        }
    }
}
