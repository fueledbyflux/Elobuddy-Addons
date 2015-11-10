using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace RivenBuddy
{
    class SpellManager
    {
        public static Dictionary<SpellSlot, Spell.SpellBase> Spells = new Dictionary<SpellSlot, Spell.SpellBase>()
        {
            {SpellSlot.Q, new Spell.Skillshot(SpellSlot.Q, 300, SkillShotType.Circular)},
            {SpellSlot.W, new Spell.Active(SpellSlot.W, 250)},
            {SpellSlot.E, new Spell.Skillshot(SpellSlot.E, 325, SkillShotType.Linear)},
            {SpellSlot.R, new Spell.Active(SpellSlot.R, 900)}
        };

        private static readonly Spell.Skillshot Q1 = new Spell.Skillshot(SpellSlot.Q, 300, SkillShotType.Circular);
        private static readonly Spell.Skillshot Q2 = new Spell.Skillshot(SpellSlot.Q, 325, SkillShotType.Circular);
        private static readonly Spell.Active W1 = new Spell.Active(SpellSlot.W, 250);
        private static readonly Spell.Active W2 = new Spell.Active(SpellSlot.W, 275);

        public static void UpdateSpells()
        {
            Spells[SpellSlot.Q] = SpellEvents.HasR ? Q2 : Q1;
            Spells[SpellSlot.W] = SpellEvents.HasR ?  W2 : W1;
        }
    }
}
