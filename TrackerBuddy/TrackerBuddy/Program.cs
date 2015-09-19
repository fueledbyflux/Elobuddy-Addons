using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using TrackerBuddy.Properties;
using Color = SharpDX.Color;
using Font = System.Drawing.Font;
using Sprite = EloBuddy.SDK.Rendering.Sprite;

namespace TrackerBuddy
{
    internal static class Program
    {
        private const int OffsetHudX = -31; //31
        private const int OffsetHudY = 16; //11

        private const int OffsetSpellsX = OffsetHudX + 22;
        private const int OffsetSpellsY = OffsetHudY + 23;

        private const int OffsetSummonersX = OffsetHudX + 4; //9
        private const int OffsetSummonersY = OffsetHudY + 2; //5

        private const int OffsetXpX = OffsetHudX + 40; //44
        private const int OffsetXpY = OffsetHudY + -49; //53

        public static readonly TextureLoader TextureLoader = new TextureLoader();

        private static Sprite MainBar { get; set; }
        private static Text Text { get; set; }

        public static readonly SpellSlot[] SpellSlots = { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R };
        private static readonly SpellSlot[] Summoners = { SpellSlot.Summoner1, SpellSlot.Summoner2 };
        private static readonly Dictionary<string, Sprite> SummonerSpells = new Dictionary<string, Sprite>();

        public static Menu Menu { get; set; }

        private static int Mode
        {
            get { return Menu["mode"].Cast<CheckBox>().CurrentValue ? 1 : 0; }
        }
        private static bool DrawAllies
        {
            get { return Menu["drawAllies"].Cast<CheckBox>().CurrentValue; }
        }
        private static bool DrawEnemies
        {
            get { return Menu["drawEnemies"].Cast<CheckBox>().CurrentValue; }
        }
        private static bool DrawText
        {
            get { return Menu["drawText"].Cast<CheckBox>().CurrentValue; }
        }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            // Create the menu
            Menu = MainMenu.AddMenu("TrackerBuddy", "trackerBuddy");
            Menu.AddGroupLabel("Tracker Buddy");
            Menu.Add("mode", new CheckBox("Low Profile Mode"));
            Menu.Add("drawText", new CheckBox("Draw Text"));
            Menu.Add("drawAllies", new CheckBox("Draw Allies"));
            Menu.Add("drawEnemies", new CheckBox("Draw Enemies"));
            Menu.AddSeparator();
            Menu.AddLabel("by fluxy");

            // Load main hud textures
            TextureLoader.Load("hud", Resources.hud);
            TextureLoader.Load("hud2", Resources.hud2);

            // Initialize main drawings
            Text = new Text("", new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold)) { Color = System.Drawing.Color.AntiqueWhite };
            MainBar = new Sprite(() => Mode == 1 ? TextureLoader["hud2"] : TextureLoader["hud"]);

            // Load summoner spells dynamically
            foreach (var summoner in HeroManager.AllHeroes.SelectMany(o => o.Spellbook.Spells.Where(s => Summoners.Contains(s.Slot))).Select(o => o.Name))
            {
                var summonerName = summoner;
                if (SummonerSpells.ContainsKey(summonerName))
                {
                    continue;
                }

                Bitmap bitmap = null;
                switch (summonerName)
                {
                    case "summonerheal":
                        bitmap = Resources.summonerheal;
                        break;
                    case "summonerdot":
                        bitmap = Resources.summonerdot;
                        break;
                    case "summonerexhaust":
                        bitmap = Resources.summonerexhaust;
                        break;
                    case "summonerflash":
                        bitmap = Resources.summonerflash;
                        break;
                    case "summonerhaste":
                        bitmap = Resources.summonerhaste;
                        break;
                    case "summonermana":
                        bitmap = Resources.summonermana;
                        break;
                    case "itemsmiteaoe":
                        bitmap = Resources.itemsmiteaoe;
                        break;
                    case "s5_summonersmiteduel":
                        bitmap = Resources.s5_summonersmiteduel;
                        break;
                    case "s5_summonersmiteplayerganker":
                        bitmap = Resources.s5_summonersmiteplayerganker;
                        break;
                    case "s5_summonersmitequick":
                        bitmap = Resources.s5_summonersmitequick;
                        break;
                    case "summonerclairvoyance":
                        bitmap = Resources.summonerclairvoyance;
                        break;
                }

                TextureLoader.Load(summonerName, bitmap);
                SummonerSpells.Add(summonerName, new Sprite(() => TextureLoader[summonerName]));
            }

            // Listen to required events
            Drawing.OnEndScene += OnDraw;
            AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += OnDomainUnload;
        }

        //private static bool draw;
        private static void OnDraw(EventArgs args)
        {
            foreach (var unit in HeroManager.AllHeroes.Where(o => !o.IsMe && o.IsHPBarRendered).Where(o => o.IsAlly ? DrawAllies : DrawEnemies))
            {
                // Summoner spells
                foreach (var summonerSpell in Summoners)
                {
                    var spell = unit.Spellbook.GetSpell(summonerSpell);
                    var cooldown = spell.CooldownExpires - Game.Time;
                    var percent = (cooldown > 0 && Math.Abs(spell.Cooldown) > float.Epsilon) ? 1f - (cooldown / spell.Cooldown) : 1f;
                    var spellPos = unit.GetSummonerOffset(spell.Slot);
                    if (SummonerSpells.ContainsKey(spell.Name))
                    {
                        var sprite = SummonerSpells[spell.Name];
                        sprite.Color = GetDrawColor(percent);
                        sprite.Draw(new Vector2(spellPos.X, spellPos.Y));
                    }

                    if (DrawText && cooldown > 0)
                    {
                        Text.TextValue = cooldown < 1 ? cooldown.ToString("0.0") : cooldown.ToString("0");
                        Text.Position = new Vector2((int) spellPos.X - 30 + Text.TextValue.Length, (int) spellPos.Y - 1);
                        Text.Draw();
                    }
                }

                // Spell cooldowns
                foreach (var slot in SpellSlots)
                {
                    var spell = unit.Spellbook.GetSpell(slot);
                    var cooldown = spell.CooldownExpires - Game.Time;
                    var percent = (cooldown > 0 && Math.Abs(spell.Cooldown) > float.Epsilon) ? 1f - (cooldown / spell.Cooldown) : 1f;
                    var spellPos = unit.GetSpellOffset(slot);

                    Drawing.DrawLine(new Vector2(spellPos.X, spellPos.Y + 3),
                        new Vector2(spellPos.X + (int) (percent * 22), spellPos.Y + 3),
                        Mode == 1 ? 6 : 11, GetDrawColor(percent));

                    if (DrawText && spell.IsLearned && cooldown > 0)
                    {
                        Text.TextValue = cooldown < 1 ? cooldown.ToString("0.0") : cooldown.ToString("0");
                        Text.Position = new Vector2((int) spellPos.X + 10 - Text.TextValue.Length * 2, (int) spellPos.Y + 12);
                        Text.Draw();
                    }
                }

                // Draw the main hud
                MainBar.Draw(new Vector2((unit.HPBarPosition.X + OffsetHudX), (unit.HPBarPosition.Y + OffsetHudY + Mode * 2)));
            }
        }

        private static Vector2 GetSpellOffset(this Obj_AI_Base hero, SpellSlot slot)
        {
            var normalPos = new Vector2(hero.HPBarPosition.X + OffsetSpellsX, hero.HPBarPosition.Y + OffsetSpellsY);
            switch (slot)
            {
                case SpellSlot.W:
                    return new Vector2(normalPos.X + 27, normalPos.Y);
                case SpellSlot.E:
                    return new Vector2(normalPos.X + 2 * 27, normalPos.Y);
                case SpellSlot.R:
                    return new Vector2(normalPos.X + 3 * 27, normalPos.Y);
            }
            return normalPos;
        }

        private static Vector2 GetSummonerOffset(this Obj_AI_Base hero, SpellSlot slot)
        {
            var normalPos = new Vector2(hero.HPBarPosition.X + OffsetSummonersX, hero.HPBarPosition.Y + OffsetSummonersY);
            return slot == SpellSlot.Summoner2 ? new Vector2(normalPos.X, normalPos.Y + 17) : normalPos;
        }

        public static System.Drawing.Color GetDrawColor(float percent)
        {
            if (percent < 0.3)
            {
                return System.Drawing.Color.Red;
            }
            if (percent < 0.6)
            {
                return System.Drawing.Color.Orange;
            }
            return percent < 1 ? System.Drawing.Color.Green : System.Drawing.Color.LawnGreen;
        }

        public static Color GetSharpDrawColor(float percent)
        {
            var color = GetDrawColor(percent);
            return new Color(color.R, color.G, color.B, color.A);
        }

        private static void OnDomainUnload(object sender, EventArgs e)
        {
            TextureLoader.Dispose();

            if (Text != null)
            {
                Text.Dispose();
                Text = null;
            }
        }
    }
}
