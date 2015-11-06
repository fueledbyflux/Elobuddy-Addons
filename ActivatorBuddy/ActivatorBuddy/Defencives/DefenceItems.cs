using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace ActivatorBuddy.Defencives
{
    class DefenceItems
    {
        // face of the mountain, locket, seraphs

        public static Dictionary<ItemId, Func<float>> ShieldHeals = new Dictionary<ItemId, Func<float>>()
        {
            {ItemId.Face_of_the_Mountain, () => (float) (Player.Instance.MaxHealth*0.1)},
            {ItemId.Locket_of_the_Iron_Solari, () => 50 + Player.Instance.Level * 10 },
            {ItemId.Archangels_Staff, () => (float) (150 + 0.2 * Player.Instance.Mana)},
        };

        public static bool ShieldItems(AIHeroClient ally)
        {
            if (ally == null || ally.IsDead || ally.Health <= 0 || !ally.IsValid) return false;
            var predHp = ally.PredictedHealth();
            if (ally.IsMe && Defence.DefenceMenu["Archangels_Staff"].Cast<CheckBox>().CurrentValue && Player.Instance.InventoryItems.Any(a => (int) a.Id == 3040) && predHp + ShieldHeals[ItemId.Archangels_Staff].Invoke() > 0) // Seraphs
            {
                var spellSlot =
                Player.Instance.InventoryItems.FirstOrDefault(a => (int) a.Id == 3040);
                if (spellSlot != null)
                {
                    Player.CastSpell(spellSlot.SpellSlot);
                    Defence.LastSpellCast = Environment.TickCount + 250;
                    return true;
                }
            }
            if (Defence.DefenceMenu["Locket_of_the_Iron_Solari"].Cast<CheckBox>().CurrentValue && (ally.IsMe && Defence.DefenceMenu["Locket_of_the_Iron_Solari_self"].Cast<CheckBox>().CurrentValue || !ally.IsMe && Defence.DefenceMenu["Locket_of_the_Iron_Solari_ally"].Cast<CheckBox>().CurrentValue) && Player.Instance.InventoryItems.Any(a => a.Id == ItemId.Locket_of_the_Iron_Solari) && predHp + ShieldHeals[ItemId.Locket_of_the_Iron_Solari].Invoke() > 0 && ally.Distance(Player.Instance) < 600) // Locket
            {
                var spellSlot =
                Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == ItemId.Locket_of_the_Iron_Solari);
                if (spellSlot != null)
                {
                    Player.CastSpell(spellSlot.SpellSlot);
                    Defence.LastSpellCast = Environment.TickCount + 250;
                    return true;
                }
            }
            if (Defence.DefenceMenu["Face_of_the_Mountain"].Cast<CheckBox>().CurrentValue && (ally.IsMe && Defence.DefenceMenu["Face_of_the_Mountain_self"].Cast<CheckBox>().CurrentValue || !ally.IsMe && Defence.DefenceMenu["Face_of_the_Mountain_ally"].Cast<CheckBox>().CurrentValue) && Player.Instance.InventoryItems.Any(a => a.Id == ItemId.Face_of_the_Mountain) && predHp + ShieldHeals[ItemId.Face_of_the_Mountain].Invoke() > 0 && ally.Distance(Player.Instance) < 700) // FOM
            {
                var spellSlot =
                Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == ItemId.Face_of_the_Mountain);
                if (spellSlot != null)
                {
                    Player.CastSpell(spellSlot.SpellSlot, ally);
                    Defence.LastSpellCast = Environment.TickCount + 250;
                    return true;
                }
            }
            if (Defence.DefenceMenu["Mikaels_Crucible"].Cast<CheckBox>().CurrentValue && Player.Instance.InventoryItems.Any(a => a.Id == ItemId.Mikaels_Crucible) && predHp + (150 + (0.1 * ally.Health)) > 0 && ally.Distance(Player.Instance) < 750) // Mikaels
            {
                var spellSlot =
                Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == ItemId.Mikaels_Crucible);
                if (spellSlot != null)
                {
                    Player.CastSpell(spellSlot.SpellSlot, ally);
                    Defence.LastSpellCast = Environment.TickCount + 250;
                    return true;
                }
            }
            return false;
        }

        public static bool CleanseItems(AIHeroClient ally)
        {
            if (ally == null || ally.IsDead || ally.Health <= 0 || !ally.IsValid) return false;
            if (!Player.Instance.InventoryItems.Any(a => (a.Id == ItemId.Quicksilver_Sash && Defence.DefenceMenu["Quicksilver_Sash"].Cast<CheckBox>().CurrentValue 
            || a.Id == ItemId.Dervish_Blade && Defence.DefenceMenu["Dervish_Blade"].Cast<CheckBox>().CurrentValue
            || a.Id == ItemId.Mercurial_Scimitar && Defence.DefenceMenu["Mercurial_Scimitar"].Cast<CheckBox>().CurrentValue
            || a.Id == ItemId.Mikaels_Crucible && ally.Distance(Player.Instance) < 750 && Defence.DefenceMenu["Mikaels_Crucible_Cleanse"].Cast<CheckBox>().CurrentValue) || !a.CanUseItem()) || !Defence.Damages[ally.NetworkId].DangerousSpells.Any(a => a.Value.IsCleanseable && (a.Value.BonusDelay == 0 || a.Key - Environment.TickCount < a.Value.BonusDelay / 2))) return false;
            var spellSlot =
                Player.Instance.InventoryItems.FirstOrDefault(
                    a => a.Id == ItemId.Dervish_Blade || a.Id == ItemId.Quicksilver_Sash || a.Id == ItemId.Mercurial_Scimitar);
            if (spellSlot != null)
            {
                Player.CastSpell(spellSlot.SpellSlot);
                foreach (var spell in Defence.Damages[ally.NetworkId].DangerousSpells.Where(a => a.Value.IsCleanseable).ToList())
                {
                    Defence.Damages[ally.NetworkId].DangerousSpells.Remove(spell.Key);
                }
                Defence.LastSpellCast = Environment.TickCount + 250;
                return true;
            }
            spellSlot = Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == ItemId.Mikaels_Crucible);
            if (spellSlot != null)
            {
                Player.CastSpell(spellSlot.SpellSlot, Player.Instance);
                foreach (var spell in Defence.Damages[ally.NetworkId].DangerousSpells.Where(a => a.Value.IsCleanseable).ToList())
                {
                    Defence.Damages[ally.NetworkId].DangerousSpells.Remove(spell.Key);
                }
                Defence.LastSpellCast = Environment.TickCount + 250;
                return true;
            }
            return false;
        }

        public static bool Zhonyas(AIHeroClient ally)
        {
            if (!ally.IsMe) return false;
            if (Player.Instance.InventoryItems.Any(a => a.Id == ItemId.Zhonyas_Hourglass && Defence.DefenceMenu["Zhonyas_Hourglass"].Cast<CheckBox>().CurrentValue 
            || ItemId.Wooglets_Witchcap == a.Id && Defence.DefenceMenu["Wooglets_Witchcap"].Cast<CheckBox>().CurrentValue) && (Player.Instance.InDanger(true) || Defence.Damages[Player.Instance.NetworkId].DangerousSpells.Any(a => a.Value.BonusDelay > 0 && (a.Key - Environment.TickCount) <= 2200 + Game.Ping) || Defence.Damages[Player.Instance.NetworkId].DangerousSpells.Any(a => a.Value.BonusDelay == 0)))
            {
                var spellSlot = Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == ItemId.Zhonyas_Hourglass || a.Id == ItemId.Wooglets_Witchcap);
                if (spellSlot != null)
                {
                    Player.CastSpell(spellSlot.SpellSlot);
                    Defence.LastSpellCast = Environment.TickCount + 250;
                    return true;
                }
            }
            return false;
        }

    }
}
