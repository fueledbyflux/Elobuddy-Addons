using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace YasuoBuddy
{
    internal class SpellManager
    {
        public static Spell.Targeted E = new Spell.Targeted(SpellSlot.E, 475);
        public static Spell.Active R = new Spell.Active(SpellSlot.R, 1200);

        public static Spell.Skillshot Q()
        {
            if (Player.Instance.HasWhirlwind())
            {
                return new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 300, 1200, 50)
                {
                    AllowedCollisionCount = int.MaxValue
                };
            }
            return new Spell.Skillshot(SpellSlot.Q, 475, SkillShotType.Linear, 300, GetNewQSpeed(), 1)
            {
                AllowedCollisionCount = int.MaxValue
            };
        }

        public static void StackQ()
        {
            if (!Q().IsReady() || Player.Instance.HasWhirlwind()) return;
            var target =
                EntityManager.Heroes.Enemies.Where(
                    a =>
                        a.Distance(Player.Instance.Position) < 475 && !Player.Instance.IsDashing() ||
                        Player.Instance.IsDashing() &&
                        DashingManager.GetPlayerPosition(300).Distance(Q().GetPrediction(a).UnitPosition) < 475).OrderBy(TargetSelector.GetPriority).FirstOrDefault();
            if (target != null)
            {
                if (!Player.Instance.IsDashing())
                {
                    Q().Cast(target);
                }
                else
                {
                    Player.CastSpell(SpellSlot.Q);
                }
            }
            var target2 =
                EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(a => a.Distance(Player.Instance.Position) <= 475 && !Player.Instance.IsDashing() ||
                        Player.Instance.IsDashing() &&
                        DashingManager.GetPlayerPosition(300).Distance(Q().GetPrediction(a).UnitPosition) < 475);
            if (target2 != null)
            {
                if (!Player.Instance.IsDashing())
                {
                    Q().Cast(target2);
                }
                else
                {
                    Player.CastSpell(SpellSlot.Q);
                }
            }
        }

        public static int GetKnockedUpTargets()
        {
            return EntityManager.Heroes.Enemies.Count(a => a.IsKnockedUp() && a.IsValidTarget(R.Range));
        }

        public static int EDelay()
        {
            return new[] {10000, 9000, 8000, 7000, 6000}[E.Level - 1];
        }

        public static int GetLowestKnockupTime()
        {
            return (int) EntityManager.Heroes.Enemies.Where(a => a.IsKnockedUp() && a.IsValidTarget(R.Range)).Select(a =>
            {
                var buff = a.Buffs.FirstOrDefault(b => b.IsKnockup || b.IsKnockback);
                return buff != null ? (buff.EndTime - Game.Time)*1000 : 0;
            }).OrderBy(a => a).FirstOrDefault();
        }

        public static int GetNewQSpeed()
        {
            return (int) (1/(1/0.5*Player.Instance.AttackSpeedMod));
        }
    }
}