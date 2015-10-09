using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace YasuoBuddy
{
    internal static class EventManager
    {
        private static readonly Dictionary<Obj_AI_Base, long> _nonDashableUnits = new Dictionary<Obj_AI_Base, long>();

        public static void Init()
        {
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            foreach (var nonDashableUnit in _nonDashableUnits.ToList().Where(nonDashableUnit => nonDashableUnit.Value < Environment.TickCount || nonDashableUnit.Key == null))
            {
                try
                {
                    _nonDashableUnits.Remove(nonDashableUnit.Key);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;
            if (args.SData.Name == Player.GetSpell(SpellSlot.E).Name)
            {
                _nonDashableUnits.Add((Obj_AI_Base) args.Target, Environment.TickCount + SpellManager.EDelay());
            }
        }

        public static bool CanDash(this Obj_AI_Base unit)
        {
            return !_nonDashableUnits.ContainsKey(unit);
        }
    }
}