using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using YasuoBuddy.EvadePlus;
using YasuoBuddy.EvadePlus.SkillshotTypes;

namespace YasuoBuddy
{
    class EEvader
    {
        public static int WallCastT;
        public static Vector2 YasuoWallCastedPos;

        public static void UpdateTask()
        {
            GameObject wall = null;
            foreach (var gameObject in ObjectManager.Get<GameObject>())
            {
                if (gameObject.IsValid &&
                    System.Text.RegularExpressions.Regex.IsMatch(
                        gameObject.Name, "_w_windwall.\\.troy",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    wall = gameObject;
                }
            }
            if (wall != null)
            {
                var level = wall.Name.Substring(wall.Name.Length - 6, 1);
                var wallWidth = (300 + 50*Convert.ToInt32(level));
                var wallDirection = (wall.Position.To2D() - YasuoWallCastedPos).Normalized().Perpendicular();
                var wallStart = wall.Position.To2D() + wallWidth/2*wallDirection;
                var wallEnd = wallStart - wallWidth*wallDirection;
                var wallPolygon = new Geometry.Polygon.Rectangle(wallStart, wallEnd, 75);

                foreach (var activeSkillshot in EvadePlus.Program.Evade.SkillshotDetector.ActiveSkillshots.Where(a => (a is LinearMissileSkillshot) && EvadeMenu.IsSkillshotW(a)))
                {
                    if (wallPolygon.IsInside(activeSkillshot.GetPosition()))
                    {
                        activeSkillshot.IsValid = false;
                    }
                }
            }

            if (EvadePlus.Program.Evade.IsHeroInDanger(Player.Instance))
            {
                if (Player.GetSpell(SpellSlot.W).State == SpellState.Ready)
                {
                    foreach (var activeSkillshot in EvadePlus.Program.Evade.SkillshotDetector.ActiveSkillshots.Where(a => a is LinearMissileSkillshot && EvadePlus.EvadeMenu.IsSkillshotW(a)))
                    {
                        if (activeSkillshot.ToPolygon().IsInside(Player.Instance))
                        {
                            Player.CastSpell(SpellSlot.W, activeSkillshot.GetPosition());
                            return;
                        }
                    }
                }
                
                if (Player.GetSpell(SpellSlot.E).State == SpellState.Ready)
                {
                    foreach (
                        var source in
                            EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                                a => a.Team != Player.Instance.Team && a.Distance(Player.Instance) < 475 && a.CanDash()))
                    {
                        if(source.GetDashPos().IsUnderTower()) continue;
                        if (EvadePlus.Program.Evade.IsPointSafe(source.GetDashPos().To2D()))
                        {
                            int count = 0;
                            for (int i = 0; i < 10; i += 47)
                            {
                                if(!EvadePlus.Program.Evade.IsPointSafe(Player.Instance.Position.Extend(source.GetDashPos(), i)))
                                {
                                    count ++;
                                }
                            }
                            if (count > 3) continue;
                            Player.CastSpell(SpellSlot.E, source);
                            break;
                        }
                    }
                    foreach (
                        var source in
                            EntityManager.Heroes.Enemies.Where(
                                a => a.IsEnemy && a.Distance(Player.Instance) < 475 && a.CanDash()))
                    {
                        if (source.GetDashPos().IsUnderTower()) continue;
                        if (EvadePlus.Program.Evade.IsPointSafe(source.GetDashPos().To2D()))
                        {
                            int count = 0;
                            for (int i = 0; i < 10; i += 47)
                            {
                                if(!EvadePlus.Program.Evade.IsPointSafe(Player.Instance.Position.Extend(source.GetDashPos(), i)))
                                {
                                    count ++;
                                }
                            }
                            if (count > 3) continue;
                            Player.CastSpell(SpellSlot.E, source);
                            break;
                        }
                    }
                }
            }
        }
    }
}
