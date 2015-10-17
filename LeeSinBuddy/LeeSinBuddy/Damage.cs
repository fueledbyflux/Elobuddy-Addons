using EloBuddy;
using EloBuddy.SDK;

namespace LeeSinBuddy
{
    internal class Damage
    {
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static double Q2Damage(Obj_AI_Base target, double subHp = 0, bool monster = false)
        {
            if (!Player.GetSpell(SpellSlot.Q).IsLearned) return 0;
            var damage = (50 + (Program.Q.Level*30)) + (0.09*_Player.FlatPhysicalDamageMod) +
                         ((target.MaxHealth - (target.Health - subHp))*0.08);
            if (monster && damage > 400)
            {
                return _Player.CalculateDamageOnUnit(target, DamageType.Physical, 400);
            }
            return _Player.CalculateDamageOnUnit(target, DamageType.Physical, (float) damage);
        }

        public static double QDamage(Obj_AI_Base target)
        {
            if (!Player.GetSpell(SpellSlot.Q).IsLearned) return 0;
            return _Player.CalculateDamageOnUnit(target, DamageType.Physical,
                (float) (new double[] {50, 80, 110, 140, 170}[Program.Q.Level - 1] + 0.9*_Player.FlatPhysicalDamageMod));
        }

        public static double EDamage(Obj_AI_Base target)
        {
            if (!Player.GetSpell(SpellSlot.E).IsLearned) return 0;
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float) new double[] {60, 95, 130, 165, 200}[Program.E.Level - 1] + 1*_Player.FlatPhysicalDamageMod);
        }

        public static double RDamage(Obj_AI_Base target)
        {
            if (!Player.GetSpell(SpellSlot.R).IsLearned) return 0;
            return _Player.CalculateDamageOnUnit(target, DamageType.Physical,
                (float) new double[] {200, 400, 600}[Program.R.Level - 1] + 2*_Player.FlatPhysicalDamageMod);
        }
    }
}