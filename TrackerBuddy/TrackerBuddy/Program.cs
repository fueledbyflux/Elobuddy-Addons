using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using SharpDX.Direct3D9;
using TrackerBuddy.Properties;
using Color = SharpDX.Color;
using Font = System.Drawing.Font;
using Sprite = EloBuddy.SDK.Rendering.Sprite;

namespace TrackerBuddy
{
    internal static class Program
    {
        public static readonly TextureLoader TextureLoader = new TextureLoader();
        private static Sprite MainBar;
        private static Line Line;
        private static Text Text;

        private const int OffsetHudX = -31;//31
        private const int OffsetHudY = 16;//11

        private const int OffsetSpellsX = OffsetHudX + 22;
        private const int OffsetSpellsY = OffsetHudY + 23;

        private const int OffsetSummonersX = OffsetHudX + 4;//9
        private const int OffsetSummonersY = OffsetHudY + 2;//5

        private const int OffsetXpX = OffsetHudX + 40; //44
        private const int OffsetXpY = OffsetHudY + -49; //53

        private static int mode;

        public static SpellSlot[] SpellSlots = {SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R};
        private static readonly SpellSlot[] Summoners = { SpellSlot.Summoner1, SpellSlot.Summoner2 };
        private static Dictionary<String, Sprite> SummonerSpells = new Dictionary<string, Sprite>();

        public static Menu menu;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            menu = MainMenu.AddMenu("TrackerBuddy", "trackerBuddy");
            menu.AddGroupLabel("Tracker Buddy");
            menu.AddSeparator();
            var mode2 = menu.Add("mode", new CheckBox("Low Profile Mode"));
            menu.Add("drawText", new CheckBox("Draw Text"));
            menu.Add("drawAllies", new CheckBox("Draw Allies"));
            menu.Add("drawEnemies", new CheckBox("Draw Enemies"));
            menu.AddSeparator();
            menu.AddLabel("by fluxy");

            mode2.OnValueChange += delegate
            {
                mode = mode2.CurrentValue ? 1 : 0;
                Line = new Line(Drawing.Direct3DDevice) { Width = mode == 1 ? 6 : 11 };
                MainBar = new Sprite(() => TextureLoader[mode == 1 ? "hud2" : "hud"]);
            };

            mode = mode2.CurrentValue ? 1 : 0;

            TextureLoader.Load("hud", Resources.hud);
            TextureLoader.Load("hud2", Resources.hud2);

            TextureLoader.Load("summonerheal", Resources.summonerheal);
            TextureLoader.Load("summonerdot", Resources.summonerdot);
            TextureLoader.Load("summonerexhaust", Resources.summonerexhaust);
            TextureLoader.Load("summonerflash", Resources.summonerflash);
            TextureLoader.Load("summonerhaste", Resources.summonerhaste);
            TextureLoader.Load("summonermana", Resources.summonermana);
            TextureLoader.Load("itemsmiteaoe", Resources.itemsmiteaoe);
            TextureLoader.Load("s5_summonersmiteduel", Resources.s5_summonersmiteduel);
            TextureLoader.Load("s5_summonersmiteplayerganker", Resources.s5_summonersmiteplayerganker);
            TextureLoader.Load("s5_summonersmitequick", Resources.s5_summonersmitequick);
            TextureLoader.Load("summonerclairvoyance", Resources.summonerclairvoyance);

            SummonerSpells.Add("summonerheal", new Sprite(TextureLoader["summonerheal"]));
            SummonerSpells.Add("summonerdot", new Sprite(() => TextureLoader["summonerdot"]));
            SummonerSpells.Add("summonerexhaust", new Sprite(() => TextureLoader["summonerexhaust"]));
            SummonerSpells.Add("summonerflash", new Sprite(() => TextureLoader["summonerflash"]));
            SummonerSpells.Add("summonerhaste", new Sprite(() => TextureLoader["summonerhaste"]));
            SummonerSpells.Add("summonermana", new Sprite(() => TextureLoader["summonermana"]));
            SummonerSpells.Add("itemsmiteaoe", new Sprite(() => TextureLoader["itemsmiteaoe"]));
            SummonerSpells.Add("s5_summonersmiteduel", new Sprite(() => TextureLoader["s5_summonersmiteduel"]));
            SummonerSpells.Add("s5_summonersmiteplayerganker", new Sprite(() => TextureLoader["s5_summonersmiteplayerganker"]));
            SummonerSpells.Add("s5_summonersmitequick", new Sprite(() => TextureLoader["s5_summonersmitequick"]));
            SummonerSpells.Add("summonerclairvoyance", new Sprite(() => TextureLoader["summonerclairvoyance"]));

            Line = new Line(Drawing.Direct3DDevice) {Width = mode == 1 ? 6 : 11};
            Text = new Text("", new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold));
            MainBar = new Sprite(() => TextureLoader[mode == 1 ? "hud2" : "hud"]);
            
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnPreReset += Drawing_OnPreReset;
            Drawing.OnPostReset += Drawing_OnPostReset;

            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_DomainUnload;
            Chat.Print("Loaded Trackerbuddy");
        }
        

        private static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            Line.Dispose();
        }

        private static void Drawing_OnPostReset(EventArgs args)
        {
            Line.OnResetDevice();
        }

        private static void Drawing_OnPreReset(EventArgs args)
        {
            Line.OnLostDevice();
        }

        //private static bool draw;
        private static void Drawing_OnDraw(EventArgs args)
        {
            int integ = 0;
            foreach (var unit in ObjectManager.Get<AIHeroClient>().Where(a => !a.IsMe && a.IsHPBarRendered))
            {
                if(!menu["drawAllies"].Cast<CheckBox>().CurrentValue && unit.IsAlly || !menu["drawEnemies"].Cast<CheckBox>().CurrentValue && unit.IsEnemy) continue;
                    //summonerspells
                    foreach (var summonerSpell in Summoners)
                {
                    var cd = unit.Spellbook.GetSpell(summonerSpell).CooldownExpires - Game.Time;
                    var percent = (cd > 0 && Math.Abs(unit.Spellbook.GetSpell(summonerSpell).Cooldown) > float.Epsilon)
                        ? 1f - (cd / unit.Spellbook.GetSpell(summonerSpell).Cooldown)
                        : 1f;
                    var spell = unit.Spellbook.GetSpell(summonerSpell);
                    var spellDraw = SummonerSpells[spell.Name];
                    if (spellDraw != null)
                    {
                        var spellPos = unit.GetSummoneroffset(spell.Slot);
                        spellDraw.Color = DrawColors(percent);
                        spellDraw.Draw(new Vector2(spellPos.X, spellPos.Y));
                    }
                }

                Line.Begin();
                foreach (var spellSlot in SpellSlots)
                {
                    var cd = unit.Spellbook.GetSpell(spellSlot).CooldownExpires - Game.Time;
                    var percent = (cd > 0 && Math.Abs(unit.Spellbook.GetSpell(spellSlot).Cooldown) > float.Epsilon)
                        ? 1f - (cd/unit.Spellbook.GetSpell(spellSlot).Cooldown)
                        : 1f;
                    var pos = unit.GetSpelloffset(spellSlot);
                    Line.Draw(new[]
                    {
                        new Vector2(pos.X, pos.Y + 3), 
                        new Vector2(unit.GetSpelloffset(spellSlot).X + (int)(percent * 22), unit.GetSpelloffset(spellSlot).Y + 3), 
                    }, unit.Spellbook.GetSpell(spellSlot).IsLearned ? DrawColor(percent) : Color.White);
                    integ ++;
                }
                Line.End();

                if (menu["drawText"].Cast<CheckBox>().CurrentValue)
                {
                    //Ability Timers
                    foreach (var ability in SpellSlots)
                    {
                        if (unit.Spellbook.CanUseSpell(ability) != SpellState.NotLearned)
                        {
                            var cd = unit.Spellbook.GetSpell(ability).CooldownExpires - Game.Time;
                            var spellPos = unit.GetSpelloffset(ability);
                            if (cd > 0)
                            {
                                var cdFrom = cd < 1 ? cd.ToString("0.0") : cd.ToString("0");
                                Text.TextValue = cdFrom;
                                Text.Position = new Vector2((int) spellPos.X + 10 - cdFrom.Length*2,
                                    (int) spellPos.Y + 12);
                                Text.Color = System.Drawing.Color.AntiqueWhite;
                                Text.Draw();
                            }
                        }
                    }

                    //Summoner Timers
                    foreach (var summoner in Summoners)
                    {
                        var cd = unit.Spellbook.GetSpell(summoner).CooldownExpires - Game.Time;
                        var spellPos = unit.GetSummoneroffset(summoner);
                        if (cd > 0)
                        {
                            var cdFrom = cd < 1 ? cd.ToString("0.0") : cd.ToString("0");
                            Text.TextValue = cdFrom;
                            Text.Position = new Vector2((int) spellPos.X - 30 + cdFrom.Length, (int) spellPos.Y - 1);
                            Text.Color = System.Drawing.Color.AntiqueWhite;
                            Text.Draw();
                        }
                    }
                }

                MainBar.Draw(new Vector2((unit.HPBarPosition.X + OffsetHudX), (unit.HPBarPosition.Y + OffsetHudY + mode * 2)));
            }
        }


        private static Vector2 GetSpelloffset(this Obj_AI_Base hero, SpellSlot slot)
        {
            var normalPos = new Vector2(hero.HPBarPosition.X + OffsetSpellsX, hero.HPBarPosition.Y + OffsetSpellsY);
            switch (slot)
            {
                case SpellSlot.Q:
                    return normalPos;
                case SpellSlot.W:
                    return new Vector2(normalPos.X + 27, normalPos.Y);
                case SpellSlot.E:
                    return new Vector2(normalPos.X + 2 * 27, normalPos.Y);
                case SpellSlot.R:
                    return new Vector2(normalPos.X + 3 * 27, normalPos.Y);
            }

            return normalPos;
        }

        private static Vector2 GetSummoneroffset(this Obj_AI_Base hero, SpellSlot slot)
        {
            var normalPos = new Vector2(hero.HPBarPosition.X + OffsetSummonersX, hero.HPBarPosition.Y + OffsetSummonersY);
            switch (slot)
            {
                case SpellSlot.Summoner1:
                    return normalPos;
                case SpellSlot.Summoner2:
                    return new Vector2(normalPos.X, normalPos.Y + 17);
            }

            return normalPos;
        }
        public static System.Drawing.Color DrawColors(float percent)
        {
            if (percent < 0.3)
            {
                return System.Drawing.Color.Red;
            }
            if (percent < 0.6)
            {
                return System.Drawing.Color.Orange;
            }
            if (percent < 0.9999)
            {
                return System.Drawing.Color.Green;
            }
            return System.Drawing.Color.LawnGreen;
        }

        public static Color DrawColor(float percent)
        {
            if (percent < 0.3)
            {
                return Color.Red;
            }
            if (percent < 0.6)
            {
                return Color.Orange;
            }
            if (percent < 0.9999)
            {
                return Color.Green;
            }
            return Color.LawnGreen;
        }
    }

}

