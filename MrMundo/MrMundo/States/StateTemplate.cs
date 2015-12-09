using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;

namespace MrMundo.States
{
    public abstract class StateTemplate
    {
        public abstract void Init();
        public abstract bool Startable();
        public abstract void Activate();
    }

    public static class StateHandler
    {
        private static readonly List<StateTemplate> States = new List<StateTemplate>
        {
            new Combo(), new Harass(), new Jungle(), new LastHit(), new WaveClear()
        }; 

        public static void Init()
        {
            foreach (var state in States)
            {
                state.Init();
            }
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            foreach (var state in States.Where(a => a.Startable()))
            {
                state.Activate();
            }
        }
    }
}
