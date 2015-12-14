using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace MrMundo.States
{
    class Harass : StateTemplate
    {
        public override void Init()
        {
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
        }

        private void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (target != null && SpellHandler.E.IsReady() && Program.HarassMenu["useEHarass"].Cast<CheckBox>().CurrentValue && EntityManager.Heroes.Enemies.Any(a => a.Name == target.Name && Player.Instance.IsInAutoAttackRange(a)))
            {
                SpellHandler.E.Cast();
            }
        }

        public override bool Startable()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Activate()
        {
            var target = TargetSelector.GetTarget(SpellHandler.Q.Range + 100, DamageType.Physical);

            if (target == null) return;

            if (SpellHandler.Q.IsReady() && Program.HarassMenu["useQHarass"].Cast<CheckBox>().CurrentValue)
            {
                SpellHandler.Q.Cast(target);
            }

            if (SpellHandler.W.IsReady() && !Program.HasW && Program.HarassMenu["useWHarass"].Cast<CheckBox>().CurrentValue)
            {
                SpellHandler.W.Cast();
            }
        }
    }
}

