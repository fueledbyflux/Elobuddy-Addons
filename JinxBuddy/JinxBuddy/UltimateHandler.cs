using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace JinxBuddy
{
    static class UltimateHandler
    {
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }
        public static bool IsKillableByR(this AIHeroClient target)
        {
            return RDamage(target) > target.Health + target.AttackShield;
        }
        public static float RDamage(AIHeroClient target)
        {
            if (!Program.R.IsLearned) return 0;
            var level = Program.R.Level - 1;

            if (target.Distance(_Player) < 1350)
            {
                return _Player.CalculateDamageOnUnit(target, DamageType.Physical,
                    (float)
                        (new double[] {25, 35, 45}[level] +
                         new double[] {25, 30, 35}[level]/100*(target.MaxHealth - target.Health) +
                         0.1*_Player.FlatPhysicalDamageMod));
            }

            return _Player.CalculateDamageOnUnit(target, DamageType.Physical,
                (float)
                    (new double[] {250, 350, 450}[level] +
                     new double[] {25, 30, 35}[level]/100*(target.MaxHealth - target.Health) +
                     1*_Player.FlatPhysicalDamageMod));
        }

        internal static float UltSpeed(Vector3 endPosition)
        {
            return 1700f;
            var distance = _Player.Distance(endPosition);

            if (distance > 1350)
            {
                var acceldifference = (distance - 1350f > 150) ? 150 : distance - 1350;
                var difference = distance - 1500f;

                return (1350f * 1700f + acceldifference * (1700f + 0.3f * acceldifference) + difference * 2200f) / distance;
            }
            return 1700;
        }
    }
}
