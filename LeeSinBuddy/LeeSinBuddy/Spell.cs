using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;

namespace LeeSinBuddy
{
    public static class SpellClass
    {
        public static SpellDataInst Instance(this Spell.SpellBase spell)
        {
            return ObjectManager.Player.Spellbook.GetSpell(spell.Slot);
        }

        public static bool SmiteQCast(Obj_AI_Base target)
        {
            if (target == null) return false;

            var pred = Program.Q.GetPrediction(target);
            if (pred.HitChance >= HitChance.High)
            {
                Program.Q.Cast(pred.CastPosition);
                return true;
            }
            if (pred.CollisionObjects.Count() == 1 && Smiter.Smite.IsReady() && Smiter.SmiteMenu["smiteQ"].Cast<CheckBox>().CurrentValue)
            {
                var unit = pred.CollisionObjects[0];
                if ((unit.IsMinion() || EntityManager.MinionsAndMonsters.CombinedAttackable.Any(a => a.NetworkId == unit.NetworkId)) && unit.Health <= Smiter.GetSmiteDamage() && unit.IsValidTarget(Smiter.Smite.Range))
                {
                    Program.Q.Cast(pred.CastPosition);
                    Core.DelayAction(delegate { Smiter.Smite.Cast(unit); }, 250);
                    return true;
                }
            }
            return false;
        }
    }
}