using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ActivatorBuddy.Items
{
    internal static class ItemManager
    {
        public static Menu PotionsMenu;
        public static Menu OffensiveMenu;
        public static Menu Offensive2Menu;
        public static Menu DefensiveMenu;
        public static Menu Summoners;
        public static Menu Cleansers;

        public static Dictionary<BuffType, string> BuffTypes = new Dictionary<BuffType, string>
        {
            {BuffType.Stun, "stunActivator"},
            {BuffType.Polymorph, "polymorphActivator"},
            {BuffType.Snare, "rootActivator"},
            {BuffType.Slow, "slowActivator"},
            {BuffType.Knockup, "knockupActivator"},
            {BuffType.Taunt, "tauntActivator"},
            {BuffType.Fear, "fearActivator"}
        };

        public static Item[] Items =
        {
            new Item("botrk", 450, CastType.Targeted, ItemId.Blade_of_the_Ruined_King, ItemType.Offensive),
            new Item("cutlass", 450, CastType.Targeted, ItemId.Bilgewater_Cutlass, ItemType.Offensive),
            new Item("tiamat", 250, CastType.SelfCast, ItemId.Tiamat_Melee_Only, ItemType.Offensive, true),
            new Item("hydra", 250, CastType.SelfCast, ItemId.Ravenous_Hydra_Melee_Only, ItemType.Offensive, true),
            new Item("gunblade", 700, CastType.Targeted, ItemId.Hextech_Gunblade, ItemType.Offensive),
            new Item("ghostblade", 1500, CastType.SelfCast, ItemId.Youmuus_Ghostblade, ItemType.Offensive),
            new Item("flaskPotion", int.MaxValue, CastType.SelfCast, ItemId.Crystalline_Flask, ItemType.Healing, false,
                "ItemCrystalFlask"),
            new Item("flaskPotion", int.MaxValue, CastType.SelfCast, ItemId.Crystalline_Flask, ItemType.ManaRestore,
                false, "ItemCrystalFlask"),
            new Item("healthPotion", int.MaxValue, CastType.SelfCast, ItemId.Health_Potion, ItemType.Healing, false,
                "RegenerationPotion"),
            new Item("manaPotion", int.MaxValue, CastType.SelfCast, ItemId.Mana_Potion, ItemType.ManaRestore, false,
                "FlaskOfCrystalWater"),
            new Item("mikaelsCleanse", int.MaxValue, CastType.SelfCast, ItemId.Mikaels_Crucible, ItemType.Cleanse),
            new Item("mercurialScimitarCleanse", int.MaxValue, CastType.SelfCast, ItemId.Mercurial_Scimitar,
                ItemType.Cleanse),
            new Item("quicksilverSashCleanse", int.MaxValue, CastType.SelfCast, ItemId.Quicksilver_Sash,
                ItemType.Cleanse)
        };

        public static List<Item> ActiveItems = new List<Item>();

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            var menu = Program.menu;

            OffensiveMenu = menu.AddSubMenu("Offensive Items", "offItems");
            OffensiveMenu.AddGroupLabel("Offensive Items");
            OffensiveMenu.AddLabel("(Activates With Combo)");
            OffensiveMenu.AddSeparator();
            OffensiveMenu.Add("botrkManager", new CheckBox("Blade Of The Ruined King", true));
            OffensiveMenu.Add("botrkManagerMinMeHP", new Slider("Self HP %", 80));
            OffensiveMenu.Add("botrkManagerMinEnemyHP", new Slider("Enemy HP HP %", 80));
            OffensiveMenu.AddSeparator();
            OffensiveMenu.Add("cutlassManager", new CheckBox("Cutlass", true));
            OffensiveMenu.Add("cutlassManagerMinMeHP", new Slider("Self HP %", 80));
            OffensiveMenu.Add("cutlassManagerMinEnemyHP", new Slider("Enemy HP HP %", 80));

            if (_Player.IsMelee)
            {
                OffensiveMenu.AddSeparator();
                OffensiveMenu.Add("tiamatManager", new CheckBox("Use Tiamat", true));
                OffensiveMenu.Add("tiamatManagerMinMeHP", new Slider("Self HP %", 99));
                OffensiveMenu.Add("tiamatManagerMinEnemyHP", new Slider("Enemy HP HP %", 99));
                OffensiveMenu.AddSeparator();
                OffensiveMenu.Add("hydraManager", new CheckBox("Use Hydra", true));
                OffensiveMenu.Add("hydraManagerMinMeHP", new Slider("Self HP %", 99));
                OffensiveMenu.Add("hydraManagerMinEnemyHP", new Slider("Enemy HP HP %", 99));
            }

            OffensiveMenu.Add("gunbladeManager", new CheckBox("Use Gunblade", true));
            OffensiveMenu.Add("gunbladeManagerMinMeHP", new Slider("Self HP %", 99));
            OffensiveMenu.Add("gunbladeManagerMinEnemyHP", new Slider("Enemy HP HP %", 99));
            OffensiveMenu.Add("ghostbladeManager", new CheckBox("Use GhostBlade", true));
            OffensiveMenu.Add("ghostbladeManagerMinMeHP", new Slider("Self HP %", 99));
            OffensiveMenu.Add("ghostbladeManagerMinEnemyHP", new Slider("Enemy HP HP %", 99));

            PotionsMenu = menu.AddSubMenu("Potions", "potions");
            PotionsMenu.AddGroupLabel("Potion Items");
            PotionsMenu.AddSeparator();
            PotionsMenu.Add("healthPotionManager", new CheckBox("Health Potion", true));
            PotionsMenu.Add("healthPotionManagerMinMeHP", new Slider("Min HP %", 20));
            PotionsMenu.AddSeparator();
            PotionsMenu.Add("manaPotionManager", new CheckBox("Mana Potion", true));
            PotionsMenu.Add("manaPotionManagerMinMeMana", new Slider("Min Mana %", 20));
            PotionsMenu.AddSeparator();
            PotionsMenu.Add("flaskPotionManager", new CheckBox("Flask", true));
            PotionsMenu.Add("flaskPotionManagerMinMeHP", new Slider("Min HP %", 20));
            PotionsMenu.Add("flaskPotionManagerMinMeMana", new Slider("Min Mana %", 20));

            Cleansers = menu.AddSubMenu("Cleansers", "cleansers");
            Cleansers.AddGroupLabel("Cleansers Settings");
            Cleansers.Add("polymorphActivator", new CheckBox("PolyMorph", true));
            Cleansers.Add("stunActivator", new CheckBox("Stun", true));
            Cleansers.Add("tauntActivator", new CheckBox("Taunt", true));
            Cleansers.Add("knockupActivator", new CheckBox("Knock-up", true));
            Cleansers.Add("fearActivator", new CheckBox("Fear", true));
            Cleansers.Add("rootActivator", new CheckBox("Root", true));
            Cleansers.Add("slowActivator", new CheckBox("Slow"));
            Cleansers.AddSeparator();
            Cleansers.Add("mikaelsCleanser", new CheckBox("Mikael's Cruicble", true));
            Cleansers.Add("mercurialScimitarCleanser", new CheckBox("Mercurial Scimitar", true));
            Cleansers.Add("quicksilverSashCleanser", new CheckBox("Quicksilver Sash", true));


            foreach (var item in Items)
            {
                if (ActiveItems.All(a => a.Name != item.Name) && _Player.InventoryItems.Any(a => a.Id == item.Id))
                {
                    ActiveItems.Add(item);
                }
            }


            Game.OnTick += Game_OnTick;
            Shop.OnBuyItem += Shop_OnBuyItem;
            Shop.OnSellItem += Shop_OnSellItem;
        }

        private static void Shop_OnSellItem(AIHeroClient sender, ShopActionEventArgs args)
        {
            Core.DelayAction(delegate
            {
                var item = ActiveItems.FirstOrDefault(a => a.Id == (ItemId) args.Id);
                if (item != null && _Player.InventoryItems.All(a => a.Id != item.Id))
                {
                    ActiveItems.Remove(item);
                }
            }, 200);
        }

        private static void Shop_OnBuyItem(AIHeroClient sender, ShopActionEventArgs args)
        {
            if (!sender.IsMe) return;

            foreach (var item in Items)
            {
                if (ActiveItems.All(a => a.Name != item.Name) && (ItemId) args.Id == item.Id)
                {
                    ActiveItems.Add(item);
                }
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            foreach (var item in ActiveItems)
            {
                if (_Player.InventoryItems.All(a => a.Id != item.Id)) continue;
                switch (item.ItemType)
                {
                    case ItemType.Offensive:
                    {
                        if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;

                        var target = (Obj_AI_Base) Orbwalker.GetTarget();

                        var menuItem = OffensiveMenu[item.Name + "Manager"].Cast<CheckBox>();
                        var menuItemMe = OffensiveMenu[item.Name + "ManagerMinMeHP"].Cast<Slider>();
                        var menuItemEnemy = OffensiveMenu[item.Name + "ManagerMinEnemyHP"].Cast<Slider>();

                        if (!target.IsValidTarget() || target.Distance(_Player) > item.Range ||
                            (item.MeleeOnly && !_Player.IsMelee) || !menuItem.CurrentValue ||
                            menuItemMe.CurrentValue <= _Player.HealthPercent ||
                            menuItemEnemy.CurrentValue <= target.HealthPercent) continue;

                        switch (item.CastType)
                        {
                            case CastType.Targeted:
                            {
                                var spellSlot = _Player.InventoryItems.FirstOrDefault(a => a.Id == item.Id);
                                if (spellSlot != null && Player.GetSpell(spellSlot.SpellSlot).IsReady)
                                {
                                    Player.CastSpell(spellSlot.SpellSlot, target);
                                }
                            }
                                break;
                            case CastType.SelfCast:
                            {
                                var spellSlot = _Player.InventoryItems.FirstOrDefault(a => a.Id == item.Id);
                                if (spellSlot != null && Player.GetSpell(spellSlot.SpellSlot).IsReady)
                                {
                                    Player.CastSpell(spellSlot.SpellSlot);
                                }
                            }
                                break;
                        }
                    }
                        break;
                    case ItemType.Healing:
                    {
                        var menuItem = PotionsMenu[item.Name + "Manager"].Cast<CheckBox>();
                        var menuItemMe = PotionsMenu[item.Name + "ManagerMinMeHP"].Cast<Slider>();
                        if (_Player.InFountain() || _Player.HasBuff(item.BuffName) || !menuItem.CurrentValue ||
                            menuItemMe.CurrentValue < _Player.HealthPercent) continue;
                        var spellSlot = _Player.InventoryItems.FirstOrDefault(a => a.Id == item.Id);
                        if (spellSlot != null && Player.GetSpell(spellSlot.SpellSlot).IsReady)
                        {
                            Player.CastSpell(spellSlot.SpellSlot);
                        }
                    }
                        break;
                    case ItemType.ManaRestore:
                    {
                        var menuItem = PotionsMenu[item.Name + "Manager"].Cast<CheckBox>();
                        var menuItemMe = PotionsMenu[item.Name + "ManagerMinMeMana"].Cast<Slider>();
                        if (_Player.InFountain() || _Player.HasBuff(item.BuffName) || !menuItem.CurrentValue ||
                            menuItemMe.CurrentValue < _Player.ManaPercent) continue;
                        var spellSlot = _Player.InventoryItems.FirstOrDefault(a => a.Id == item.Id);
                        if (spellSlot != null && Player.GetSpell(spellSlot.SpellSlot).IsReady)
                        {
                            Player.CastSpell(spellSlot.SpellSlot);
                        }
                    }
                        break;

                    case ItemType.Cleanse:
                    {
                        if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;

                        foreach (var buffHandle in _Player.Buffs)
                        {
                            if (BuffTypes.ContainsKey(buffHandle.Type) &&
                                Cleansers[BuffTypes[buffHandle.Type]].Cast<CheckBox>().CurrentValue)
                            {
                                var spellSlot = _Player.InventoryItems.FirstOrDefault(a => a.Id == item.Id);
                                if (spellSlot != null && Player.GetSpell(spellSlot.SpellSlot).IsReady)
                                {
                                    Player.CastSpell(spellSlot.SpellSlot, _Player);
                                }
                            }
                        }
                    }
                        break;
                }
            }
        }

        public static bool HasBuff(this Obj_AI_Base unit, string s)
        {
            return
                unit.Buffs.Any(
                    a =>
                        a.Name.ToLower().Contains(s.ToLower()) || a.DisplayName.ToLower().Contains(s.ToLower()));
        }

        public static bool InFountain(this AIHeroClient hero)
        {
            var fountainRange = 1050;
            return hero.IsVisible
                   && ObjectManager.Get<Obj_SpawnPoint>().Any(sp => hero.Distance(sp.Position) < fountainRange);
        }
    }
}