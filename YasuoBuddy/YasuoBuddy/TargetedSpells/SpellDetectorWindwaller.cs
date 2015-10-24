using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace YasuoBuddy.TargetedSpells
{
    class SpellDetectorWindwaller
    {
        private static Menu TargetedMenu;
        public static void Init()
        {
            TargetedMenu = EvadePlus.EvadeMenu.MainMenu.AddSubMenu("Targeted Skills");
            TargetedMenu.AddGroupLabel("Targeted Skills");
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                TargetedMenu.AddSeparator();
                TargetedMenu.AddLabel(enemy.ChampionName);
                foreach (var spell in TargetSpellDatabase.Spells.Where(a => a.Type == SpellType.Targeted && a.ChampionName == enemy.ChampionName.ToLower()))
                {
                    TargetedMenu.Add(spell.Name + "/e", new CheckBox(spell.Name + ": " + spell.Spellslot));
                }
            }

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsAlly || !(sender is AIHeroClient) || args.Target == null || !args.Target.IsMe || Player.GetSpell(SpellSlot.W).State != SpellState.Ready) return;
            var spell = TargetSpellDatabase.GetByName(args.SData.Name);
            if (spell != null && TargetedMenu[spell.Name + "/e"] != null && TargetedMenu[spell.Name + "/e"].Cast<CheckBox>().CurrentValue)
            {
                Core.DelayAction(delegate { Player.CastSpell(SpellSlot.W, sender.Position); },
                    (int)
                        ((Player.Instance.Distance(sender) - 100/args.SData.MissileSpeed > 0
                            ? args.SData.MissileSpeed
                            : 2000)*1000 > 1
                            ? (Player.Instance.Distance(sender) - 100/args.SData.MissileSpeed > 0
                                ? args.SData.MissileSpeed
                                : 2000)*1000
                            : 0));
            }
        }
    }
}
