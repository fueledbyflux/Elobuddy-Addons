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
    class Jungle : StateTemplate
    {
        public override void Init()
        {

        }

        public override bool Startable()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Activate()
        {
            if (!SpellHandler.Q.IsReady() || !Program.FarmMenu["useQJNG"].Cast<CheckBox>().CurrentValue) return;

            if (EntityManager.MinionsAndMonsters.Monsters.OrderByDescending(a => a.MaxHealth).Any(a => a.Distance(Player.Instance.Position) <= SpellHandler.Q.Range && SpellHandler.Q.Cast(a)))
            {
                return;
            }
        }
    }
}
