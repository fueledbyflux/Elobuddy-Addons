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
    class LastHit : StateTemplate
    {
        public override void Init()
        {

        }

        public override bool Startable()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Activate()
        {
            if (!SpellHandler.Q.IsReady() || !Program.FarmMenu["useQLH"].Cast<CheckBox>().CurrentValue) return;
            
            if (EntityManager.MinionsAndMonsters.EnemyMinions.OrderByDescending(a => a.MaxHealth).Any(
                    a => a.Health <= Program.QDamage(a) && SpellHandler.Q.Cast(a)))
            {
                return;
            }
        }
    }
}
