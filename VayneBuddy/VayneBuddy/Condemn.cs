using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace VayneBuddy
{
    static class Condemn
    {

        public static AIHeroClient _Player => ObjectManager.Player;
        public static long LastCheck;
        public static int CheckCount;

        public static bool IsCondemable(this AIHeroClient unit, Vector2 pos = new Vector2())
        {
            if (unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield) || LastCheck + 50 > Environment.TickCount || _Player.IsDashing()) return false;
            Program.CorrectPoints = new List<Vector2>();
            Program.Points = new List<Vector2>();
            if (!pos.IsValid()) pos = ObjectManager.Player.Position.To2D();
            int wallCount = 0;
            int pushDistance = Program.CondemnMenu["pushDistance"].Cast<Slider>().CurrentValue;

            for (int i = 0; i < pushDistance; i += 20)
            {
                var unitPos = Prediction.Position.PredictUnitPosition(unit, 250);
                var cell = pos.Extend(unitPos, unitPos.Distance(pos) + i);
                if (cell.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall))
                {
                    Program.CorrectPoints.Add(cell);
                    wallCount++;
                }
                else
                {
                    Program.Points.Add(cell);
                }
            }

            if (CheckCount >= 2 && wallCount > 2)
            {
                CheckCount = 0;
                LastCheck = Environment.TickCount;
                return true;
            }

            if (wallCount > 2)
            {
                CheckCount++;
            }
            else
            {
                CheckCount = 0;
            }
            LastCheck = Environment.TickCount;
            return false;
        }

        public static Vector2 GetFirstNonWallPos(Vector2 startPos, Vector2 endPos)
        {
            int distance = 0;
            for (int i = 0; i < Program.CondemnMenu["pushDistance"].Cast<Slider>().CurrentValue; i += 20)
            {
                var cell = startPos.Extend(endPos, endPos.Distance(startPos) + i).ToNavMeshCell().CollFlags;
                if (cell.HasFlag(CollisionFlags.Wall) || cell.HasFlag(CollisionFlags.Building))
                {
                    distance = i - 20;
                }
            }
            return startPos.Extend(endPos, distance + endPos.Distance(startPos));
        }

        public static AIHeroClient CondemnTarget()
        {
            var min = Program.CondemnPriorityMenu["minSliderAutoCondemn"].Cast<Slider>().CurrentValue;
            return
                ObjectManager.Get<AIHeroClient>()
                    .Where(
                        a =>
                            a.IsEnemy && a.IsValidTarget(Program.E.Range) && a.IsCondemable() &&
                            Program.CondemnPriorityMenu[a.ChampionName + "priority"].Cast<Slider>().CurrentValue >= min)
                    .OrderByDescending(a => Program.CondemnPriorityMenu[a.ChampionName + "priority"].Cast<Slider>().CurrentValue)
                    .FirstOrDefault();
        }
    }

}
