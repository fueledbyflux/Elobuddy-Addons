using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace VayneBuddy
{
    public static class Utility
    {
        public static bool IsUnderAlliedTurret(this Obj_AI_Base unit)
        {
            return ObjectManager.Get<Obj_AI_Turret>().Where(a => a.Team != unit.Team && a.Health > 0).Any(turret => turret.Distance(unit) < 1000);
        }

        public static bool HasWBuff(this Obj_AI_Base target)
        {
            return target.GetBuffCount("VayneSilverDebuff") == 2;
        }
    }
}
