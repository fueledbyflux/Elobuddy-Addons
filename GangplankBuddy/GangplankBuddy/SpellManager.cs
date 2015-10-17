using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace GangplankBuddy
{
    class SpellManager
    {
        public static Spell.Targeted Q = new Spell.Targeted(SpellSlot.Q, 625);
        public static Spell.Active W = new Spell.Active(SpellSlot.W);
        public static Spell.Skillshot E = new Spell.Skillshot(SpellSlot.E, 1200, SkillShotType.Circular);
        public static Spell.Skillshot R = new Spell.Skillshot(SpellSlot.R, uint.MaxValue, SkillShotType.Circular);
    }
}
