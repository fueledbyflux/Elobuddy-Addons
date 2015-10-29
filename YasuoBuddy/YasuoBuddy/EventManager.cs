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
        private static readonly Dictionary<Obj_AI_Base, long> NonDashableUnits = new Dictionary<Obj_AI_Base, long>();

        public static void Init()
        {
            return;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            foreach (var nonDashableUnit in NonDashableUnits.ToList().Where(nonDashableUnit => nonDashableUnit.Value < Environment.TickCount || nonDashableUnit.Key == null))
            {
                NonDashableUnits.Remove(nonDashableUnit.Key);
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {   
            if (!sender.IsMe) return;
            if (args.SData.Name == Player.GetSpell(SpellSlot.E).Name)
            {
                try { 
                    NonDashableUnits.Add((Obj_AI_Base) args.Target, Environment.TickCount + SpellManager.EDelay());
                }
                catch(Exception)
                {
                    NonDashableUnits[sender] = Environment.TickCount + SpellManager.EDelay();
                }
            }
        }

        public static bool CanDash(this Obj_AI_Base unit)
        {
            return !unit.HasBuff("YasuoDashWrapper");
        }
    }
}