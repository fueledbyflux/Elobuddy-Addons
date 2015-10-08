using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;

namespace ActivatorBuddy.Summoner_Spells
{
    internal class SummonerSpells
    {
        private static Spell.Targeted _ignite;
        private static Spell.Targeted _heal;
        private static Spell.Targeted _exhaust;
        public static Spell.Targeted Smite;
        private static Menu _summonerMenu;
        private static Menu _smiteMenu;

        private static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            _summonerMenu = Program.menu.AddSubMenu("Summoner Spells");
            if (HasSpell("summonerdot"))
            {
                _summonerMenu.AddGroupLabel("Ignite Settings");
                _summonerMenu.AddSeparator();
                _summonerMenu.Add("useIgnite", new CheckBox("Use Ignite"));
                _summonerMenu.Add("comboOnlyIgnite", new CheckBox("Combo Only"));
                _summonerMenu.Add("drawIngiteRange", new CheckBox("Draw Ignite Range"));
                _summonerMenu.AddSeparator();
                _ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);
                Game.OnTick += IgniteEvent;
                Chat.Print("Activator: Ignite Loaded.", Color.LimeGreen);
            }
            if (HasSpell("summonerexhaust"))
            {
                _summonerMenu.AddGroupLabel("Exhaust Settings");
                _summonerMenu.AddSeparator();
                _summonerMenu.Add("useExhaust", new CheckBox("Use Exhaust"));
                _summonerMenu.Add("comboOnlyExhaust", new CheckBox("Combo Only"));
                _summonerMenu.Add("drawExhaustRange", new CheckBox("Draw Exhaust Range"));
                foreach (var source in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy))
                {
                    _summonerMenu.Add(source.ChampionName + "exhaust",
                        new CheckBox("Exhaust " + source.ChampionName, false));
                }
                _summonerMenu.AddSeparator();
                _exhaust = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerexhaust"), 650);
                Game.OnTick += ExhaustEvent;
                Chat.Print("Activator: Exhaust Loaded.", Color.LimeGreen);
            }
            if (HasSpell("summonerheal"))
            {
                _summonerMenu.AddGroupLabel("Heal Settings");
                _summonerMenu.AddSeparator();
                _summonerMenu.Add("useHeal", new CheckBox("Use Heal"));
                _summonerMenu.Add("comboOnlyHeal", new CheckBox("Combo Only"));
                _summonerMenu.Add("drawHealRange", new CheckBox("Draw Heal Range"));
                _summonerMenu.Add("selfHealPercent", new Slider("Self Heal %", 40));
                _summonerMenu.Add("allyHealPercent", new Slider("Ally Heal %", 20));
                foreach (var source in ObjectManager.Get<AIHeroClient>().Where(a => a.IsAlly))
                {
                    _summonerMenu.Add(source.ChampionName + "heal", new CheckBox("Heal " + source.ChampionName, false));
                }
                _summonerMenu.AddSeparator();
                _heal = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerheal"), 850);
                Game.OnTick += HealEvent;
                Chat.Print("Activator: Heal Loaded.", Color.LimeGreen);
            }
            if (HasSpell("smite"))
            {
                _smiteMenu = Program.menu.AddSubMenu("Smite Settings");
                _smiteMenu.AddGroupLabel("Camps");
                _smiteMenu.AddLabel("Epics");
                _smiteMenu.Add("SRU_Baron", new CheckBox("Bazza (Baron)"));
                _smiteMenu.Add("SRU_Dragon", new CheckBox("Dazza (Dragon)"));
                _smiteMenu.AddLabel("Buffs");
                _smiteMenu.Add("SRU_Blue", new CheckBox("Blue"));
                _smiteMenu.Add("SRU_Red", new CheckBox("Red"));
                _smiteMenu.AddLabel("Small Camps");
                _smiteMenu.Add("SRU_Gromp", new CheckBox("Gromp", false));
                _smiteMenu.Add("SRU_Murkwolf", new CheckBox("Murkwolf", false));
                _smiteMenu.Add("SRU_Krug", new CheckBox("Krug", false));
                _smiteMenu.Add("SRU_Razorbeak", new CheckBox("Razerbeak", false));
                _smiteMenu.Add("Sru_Crab", new CheckBox("Skuttles", false));
                _smiteMenu.AddSeparator();
                _smiteMenu.Add("drawSmiteRange", new CheckBox("Draw Smite Range"));
                _smiteMenu.Add("smiteActive",
                    new KeyBind("Smite Active (toggle)", true, KeyBind.BindTypes.PressToggle, 'H'));
                _smiteMenu.AddSeparator();
                _smiteMenu.Add("useSlowSmite", new CheckBox("KS with Slow Smite"));
                _smiteMenu.Add("comboWithDuelSmite", new CheckBox("Combo With Duel Smite"));

                Smite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonersmite"), 500);
                Game.OnTick += SmiteEvent;
                Chat.Print("Activator: Smite Loaded.", Color.LimeGreen);
            }
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void SmiteEvent(EventArgs args)
        {
            if (_smiteMenu["smiteActive"].Cast<KeyBind>().CurrentValue)
            {
                Smiter.SetSmiteSlot();
                var unit =
                    ObjectManager.Get<Obj_AI_Base>()
                        .Where(
                            a =>
                                Smiter.SmiteableUnits.Contains(a.BaseSkinName) && a.Health < Smiter.GetSmiteDamage() &&
                                _smiteMenu[a.BaseSkinName].Cast<CheckBox>().CurrentValue)
                        .OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault();

                if (unit != null)
                {
                    Smite.Cast(unit);
                    return;
                }
            }
            if (_smiteMenu["useSlowSmite"].Cast<CheckBox>().CurrentValue &&
                Smite.Handle.Name == "s5_summonersmiteplayerganker")
            {
                foreach (
                    var target in
                        ObjectManager.Get<AIHeroClient>()
                            .Where(h => h.IsEnemy && h.IsValidTarget(Smite.Range) && h.Health <= 20 + 8 * _Player.Level))
                {
                    Smite.Cast(target);
                    Chat.Print("Smited son");
                    return;
                }
            }
            if (_smiteMenu["comboWithDuelSmite"].Cast<CheckBox>().CurrentValue &&
                Smite.Handle.Name == "s5_summonersmiteduel" &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                foreach (
                    var target in
                        ObjectManager.Get<AIHeroClient>()
                            .Where(h => h.IsValidTarget(Smite.Range)).OrderBy(a => a.Distance(_Player)))
                {
                    Smite.Cast(target);
                    return;
                }
            }
        }

        #region Heal

        private static void HealEvent(EventArgs args)
        {
            if (!_summonerMenu["useHeal"].Cast<CheckBox>().CurrentValue || _summonerMenu["comboOnlyHeal"].Cast<CheckBox>().CurrentValue &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                return;
            foreach (
                var source in
                    from source in
                        ObjectManager.Get<AIHeroClient>().Where(a => a.IsAlly && a.Distance(_Player) < _heal.Range)
                    let minHealSlider = source.IsMe
                        ? _summonerMenu["selfHealPercent"].Cast<Slider>().CurrentValue
                        : _summonerMenu["allyHealPercent"].Cast<Slider>().CurrentValue
                    where
                        _summonerMenu[source.ChampionName + "heal"].Cast<CheckBox>().CurrentValue &&
                        source.HealthPercent <= minHealSlider
                    select source)
            {
                _heal.Cast(source);
                return;
            }
        }

        #endregion

        #region Exhaust

        private static void ExhaustEvent(EventArgs args)
        {
            if (!_summonerMenu["useExhaust"].Cast<CheckBox>().CurrentValue || _summonerMenu["comboOnlyExhaust"].Cast<CheckBox>().CurrentValue &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                return;
            foreach (
                var enemy in
                    ObjectManager.Get<AIHeroClient>()
                        .Where(a => a.IsEnemy && a.IsValidTarget(_exhaust.Range))
                        .Where(enemy => _summonerMenu[enemy.ChampionName + "exhaust"].Cast<CheckBox>().CurrentValue))
            {
                if (enemy.IsFacing(_Player))
                {
                    if (!(_Player.HealthPercent < 50)) continue;
                    _exhaust.Cast(enemy);
                    return;
                }
                if (!(enemy.HealthPercent < 50)) continue;
                _exhaust.Cast(enemy);
                return;
            }
        }

        #endregion

        #region Ignite

        private static void IgniteEvent(EventArgs args)
        {
            if (!_summonerMenu["useIgnite"].Cast<CheckBox>().CurrentValue || _summonerMenu["comboOnlyIgnite"].Cast<CheckBox>().CurrentValue &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;
            foreach (
                var source in
                    ObjectManager.Get<AIHeroClient>()
                        .Where(
                            a =>
                                a.IsEnemy && a.IsValidTarget(_ignite.Range) &&
                                a.Health < 50 + 20 * _Player.Level - (a.HPRegenRate / 5 * 3)))
            {
                _ignite.Cast(source);
                return;
            }
        }

        #endregion

        #region Drawing

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (HasSpell("summonerexhaust") && _summonerMenu["drawExhaustRange"].Cast<CheckBox>().CurrentValue &&
                _exhaust.IsReady())
            {
                Circle.Draw(SharpDX.Color.OrangeRed, _exhaust.Range, _Player.Position);
            }
            if (HasSpell("summonerheal") && _summonerMenu["drawHealRange"].Cast<CheckBox>().CurrentValue &&
                _heal.IsReady())
            {
                Circle.Draw(SharpDX.Color.DeepSkyBlue, _heal.Range, _Player.Position);
            }
            if (HasSpell("smite") && _smiteMenu["drawSmiteRange"].Cast<CheckBox>().CurrentValue && _smiteMenu["smiteActive"].Cast<KeyBind>().CurrentValue)
            {
                Circle.Draw(SharpDX.Color.Yellow, Smite.Range, _Player.Position);
            }
            if (HasSpell("summonerdot") && _summonerMenu["drawIngiteRange"].Cast<CheckBox>().CurrentValue &&
                _ignite.IsReady())
            {
                Circle.Draw(SharpDX.Color.Red, _ignite.Range, _Player.Position);
            }
        }

        #endregion

        #region Util

        public static bool HasSpell(string s)
        {
            return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }

        #endregion
    }
}