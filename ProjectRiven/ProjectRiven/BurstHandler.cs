using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;

namespace ProjectRiven
{
    class BurstHandler
    {
        public static AIHeroClient Target;
        public static SpellSlot Flash;
        public static bool ComboFinished = true;

        public static void Init()
        {
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            Flash = Player.Instance.GetSpellSlotFromName("summonerflash");
            if (Riven.BurstActive)
            {
                Orbwalker.OrbwalkTo(Game.CursorPos);
                Target = TargetSelector.GetTarget(400 + Riven.E.Range + Riven.W.Range, DamageType.Physical);
                Orbwalker.ForcedTarget = Target;
                if (Target != null && Riven.E.IsReady() && Riven.R.IsReady() && Riven.W.IsReady() && Riven.E.IsReady() && ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
                {
                    Player.CastSpell(SpellSlot.E, Target.Position);
                    return;
                }
                if (Target != null) return;
            }
            Orbwalker.ForcedTarget = null;
            Target = null;
            ComboFinished = true;
        }

        private static void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || !Riven.BurstActive || Target == null) return;

            if (args.SData.Name.ToLower().Contains("itemtiamatcleave"))
            {
                Riven.R.Cast(Target);
                return;
            }

            if (args.SData.IsAutoAttack())
            {
                if (ItemHandler.Hydra != null && ItemHandler.Hydra.IsReady())
                {
                    ItemHandler.Hydra.Cast();
                    return;
                }
            }

            if (args.SData.Name.ToLower().Contains("summonerflash"))
            {
                Player.CastSpell(SpellSlot.W);
                return;
            }

            if (args.SData.Name.ToLower().Contains(Riven.Q.Name.ToLower()))
            {
                ComboFinished = true;
                return;
            }

            if (args.SData.Name.ToLower().Contains(Riven.W.Name.ToLower()))
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                Orbwalker.ForcedTarget = Target;
                return;
            }

            if (args.SData.Name.ToLower().Contains(Riven.E.Name.ToLower()))
            {
                ComboFinished = false;
                if (!Player.Instance.HasBuff("RivenFengShuiEngine"))
                {
                    Player.CastSpell(SpellSlot.R);
                }
                return;
            }

            if (args.SData.Name.ToLower().Contains(Riven.R.Name.ToLower()))
            {
                if (Player.Instance.HasBuff("RivenFengShuiEngine"))
                {
                    Player.CastSpell(SpellSlot.Q, Target.Position);
                }
                else
                {
                    var flash = Player.GetSpell(Flash);
                    if (flash != null && flash.State == SpellState.Ready)
                    {
                        Player.CastSpell(flash.Slot, Target.Position);
                    }
                }
                return;
            }
        }
    }
}
