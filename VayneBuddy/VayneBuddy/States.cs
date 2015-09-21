using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace VayneBuddy
{
    class States
    {
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Combo()
        {
            var target = (Events.AAedTarget != null && Events.AAedTarget.IsValidTarget(_Player.GetAutoAttackRange(Events.AAedTarget)) && Events.AAedTarget is AIHeroClient && Events.AaStacks == 2) ? (AIHeroClient) Events.AAedTarget : TargetSelector2.GetTarget((int)_Player.GetAutoAttackRange(), DamageType.Physical);
            var condemnTarget = TargetSelector2.GetTarget((int)_Player.GetAutoAttackRange() + 300, DamageType.Physical);
            Orbwalker.ForcedTarget = target;
            if (!target.IsValidTarget() || Orbwalker.IsAutoAttacking) return;

            if (Program.E.IsReady() && target.IsValidTarget(Program.E.Range) && target.IsCondemable() &&
                Program.CondemnMenu["condemnCombo"].Cast<CheckBox>().CurrentValue)
            {
                Program.E.Cast(target);
                if (_Player.Spellbook.GetSpell(SpellSlot.Trinket).IsReady &&
                    _Player.Spellbook.GetSpell(SpellSlot.Trinket).SData.Name.ToLower().Contains("totem"))
                {
                    Core.DelayAction(delegate
                    {
                        if (Program.CondemnMenu["condemnComboTrinket"].Cast<CheckBox>().CurrentValue)
                        {
                            var pos = Condemn.GetFirstNonWallPos(_Player.Position.To2D(), target.Position.To2D());
                            if (!pos.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Grass)) return;
                            Player.CastSpell(SpellSlot.Trinket,
                                pos.To3D());
                        }
                    }, 200);
                }
            }

            if (Program.CondemnMenu["smartVsCheap"].Cast<CheckBox>().CurrentValue)
            {
                if (Program.E.IsReady() && Program.Q.IsReady() && condemnTarget.IsValidTarget(Program.E.Range + 300))
                {
                    if (condemnTarget.IsCondemable(_Player.Position.Extend(Game.CursorPos, 300)))
                    {
                        Program.Q.Cast(Game.CursorPos);
                    }
                    if (condemnTarget.IsCondemable(_Player.Position.Extend(target.Position, 300)))
                    {
                        Program.Q.Cast(condemnTarget.Position);
                    }
                }
                return;
            }

            if (Program.Q.IsReady() && _Player.Position.Extend(Game.CursorPos, 300).Distance(target) > 100)
            {
                Program.Q.Cast(Game.CursorPos);
            }

            if (Program.E.IsReady())
            {
                var direction = ObjectManager.Player.Direction.To2D().Perpendicular();
                for (var i = 0f; i < 360f; i += 30)
                {
                    var angleRad = Geometry.DegreeToRadian(i);
                    var rotatedPosition = ObjectManager.Player.Position.To2D() + (300f * direction.Rotated(angleRad));
                    if (condemnTarget.IsCondemable(rotatedPosition))
                    {
                        Program.Q.Cast(rotatedPosition.To3D());
                        break;
                    }
                }
            }
        }

        public static void Harass()
        {
            var target = TargetSelector2.GetTarget((int)_Player.AttackRange + 500, DamageType.Physical);
            Orbwalker.ForcedTarget = target;
            if (!target.IsValidTarget()) return;

            if (Program.Q.IsReady() && _Player.Position.Extend(Game.CursorPos, 300).Distance(target) > 100)
            {
                Program.Q.Cast(Game.CursorPos);
            }
        }
    }

}
