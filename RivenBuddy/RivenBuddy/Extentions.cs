using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace RivenBuddy
{
    static class Extentions
    {
        public static bool inRange(this Spell.SpellBase s, Obj_AI_Base target)
        {
            return ObjectManager.Player.Distance(target) <=
                   s.Range;
        }

        public static float getTrueRange(this Spell.SpellBase s, Obj_AI_Base target)
        {
            return s.Range;
        }
    }
}
