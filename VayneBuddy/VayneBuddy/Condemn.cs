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
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static long LastCheck;
        public static int CheckCount;
        public static Spell.Skillshot ESpell;

        public static bool IsCondemable(this AIHeroClient unit, Vector2 pos = new Vector2())
        {
            if (unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield) || LastCheck + 50 > Environment.TickCount || _Player.IsDashing()) return false;
            var prediction = ESpell.GetPrediction(unit);
            var predictionsList = pos.IsValid() ? new List<Vector3>() {pos.To3D()} :  new List<Vector3>
                        {
                            unit.ServerPosition,
                            unit.Position,
                            prediction.CastPosition,
                            prediction.UnitPosition
                        };

            var wallsFound = 0;
            Program.Points = new List<Vector2>();
            foreach (var position in predictionsList)
            {
                for (var i = 0; i < Program.CondemnMenu["pushDistance"].Cast<Slider>().CurrentValue; i += (int) unit.BoundingRadius)
                {
                    var cPos = _Player.Position.Extend(position, _Player.Distance(position) + i).To3D();
                    Program.Points.Add(cPos.To2D());
                    if (NavMesh.GetCollisionFlags(cPos).HasFlag(CollisionFlags.Wall) ||
                            NavMesh.GetCollisionFlags(cPos).HasFlag(CollisionFlags.Building))
                    {
                        wallsFound++;
                        break;
                    }
                }
            }
            if ((wallsFound/ predictionsList.Count) >= Program.CondemnMenu["condemnPercent"].Cast<Slider>().CurrentValue/100f)
            {
                return true;
            }
            
            return false;
        }

        public static Vector2 GetFirstNonWallPos(Vector2 startPos, Vector2 endPos)
        {
            int distance = 0;
            for (int i = 0; i < Program.CondemnMenu["pushDistance"].Cast<Slider>().CurrentValue; i += 20)
            {
                var cell = startPos.Extend(endPos, endPos.Distance(startPos) + i);
                if (NavMesh.GetCollisionFlags(cell).HasFlag(CollisionFlags.Wall) ||
                            NavMesh.GetCollisionFlags(cell).HasFlag(CollisionFlags.Building))
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
