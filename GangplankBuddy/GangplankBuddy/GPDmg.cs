using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace GangplankBuddy
{
    class GPDmg
    {
        public static float QDamage(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, (float) (new double[] {20, 45, 70, 95, 120}[Player.Instance.Spellbook.GetSpell(SpellSlot.Q).Level - 1] + 1*(Player.Instance.TotalAttackDamage)));
        }
        public static double EDamage(Obj_AI_Base target, float dmg)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, (float)( !target.IsMinion() ? (new double[] { 80, 110, 140, 170, 200 }[Player.Instance.Spellbook.GetSpell(SpellSlot.E).Level - 1]) : 0 + (dmg)));
        }
    }
}
