using EloBuddy;
using EloBuddy.SDK;

namespace LeeSinBuddy
{
    public static class SpellClass
    {
        public static SpellDataInst Instance(this Spell.SpellBase spell)
        {
            return ObjectManager.Player.Spellbook.GetSpell(spell.Slot);
        }
    }
}