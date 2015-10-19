using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace VayneBuddy
{
    class Damages
    {
        public static double QDamage(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                (float)
                    (new[] {0.3, 0.35, 0.4, 0.45, 0.5}[
                        Player.Instance.Spellbook.GetSpell(SpellSlot.Q).Level - 1])*
                (Player.Instance.TotalAttackDamage));
        }
    }
}
