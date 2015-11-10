using EloBuddy;
using EloBuddy.SDK;

namespace RivenBuddy
{
    internal static class Extentions
    {
        public static bool InRange(this Spell.SpellBase s, Obj_AI_Base target)
        {
            return ObjectManager.Player.Distance(target) <= s.Range;
        }

        public static float GetTrueRange(this Spell.SpellBase s, Obj_AI_Base target)
        {
            return s.Range;
        }
    }
}