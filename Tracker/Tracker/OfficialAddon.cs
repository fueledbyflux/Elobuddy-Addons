#region References

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using SharpDX.Direct3D9;
using Tracker.Properties;
using Font = SharpDX.Direct3D9.Font;
#endregion

namespace Tracker
{
    public static class OfficialAddon
    {
        #region Offsets
        private const int OffsetHudX = -31;//31
        private const int OffsetHudY = 16;//11

        private const int OffsetSpellsX = OffsetHudX + 22;
        private const int OffsetSpellsY = OffsetHudY + 23;

        private const int OffsetSummonersX = OffsetHudX + 4;//9
        private const int OffsetSummonersY = OffsetHudY + 2;//5

        private const int OffsetXpX = OffsetHudX + 40; //44
        private const int OffsetXpY = OffsetHudY + -49; //53
        #endregion
        #region Sprites
        private static Sprite _sprite;
        private static Texture _hudTexture;
        private static Texture _onCd;
        private static Texture _isReady;
        private static Texture _xpBar;
        private static Texture _summonerCd;
        private static readonly Dictionary<int, Texture> Summoner1 = new Dictionary<int, Texture>();
        private static readonly Dictionary<int, Texture> Summoner2 = new Dictionary<int, Texture>();
        #endregion
        #region Text
        private static Font _abilityText;
        #endregion
        #region Abilities
        private static readonly SpellSlot[] Abilities = { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R };
        private static readonly SpellSlot[] Summoners = { SpellSlot.Summoner1, SpellSlot.Summoner2 };
        #endregion

        public static void Initialize()
        {

            //Drawing Events
            Drawing.OnEndScene += args1 => OnDraw();
            Drawing.OnPreReset += args1 => OnPreReset();
            Drawing.OnPostReset += args1 => OnPostReset();

            //AppDomain Events
            AppDomain.CurrentDomain.DomainUnload += (sender1, args1) => DomainUnload();
            AppDomain.CurrentDomain.ProcessExit += (sender1, args1) => DomainUnload();

            _sprite = new Sprite(Drawing.Direct3DDevice);

            #region LoadTextures

            _hudTexture = Texture.FromMemory(
                Drawing.Direct3DDevice,
                (byte[]) new ImageConverter().ConvertTo(Resources.hud, typeof (byte[])), 153, 
                36, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);

            _onCd = Texture.FromMemory(
                Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.OnCD, typeof(byte[])), 22,
                9, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);

            _isReady = Texture.FromMemory(
                Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.IsReady, typeof(byte[])), 22,
                9, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);

            _xpBar = Texture.FromMemory(
                Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.xpBar, typeof(byte[])), 104,
                3, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);

            _summonerCd = Texture.FromMemory(
                            Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.summonercd, typeof(byte[])),
                            13, 13, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            
            #region SummonerTextures
            foreach (var hero in ObjectManager.Get<AIHeroClient>())
            {
                foreach (var summoner in Summoners)
                {
                    var spell = hero.Spellbook.GetSpell(summoner);
                    var texture = GetSummonerSprite(spell);

                    if (spell.Slot == SpellSlot.Summoner1)
                    {
                        Summoner1.Add(hero.NetworkId, Texture.FromMemory(
                            Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(texture, typeof(byte[])),
                            13,
                            13, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0));
                    }
                    else
                    {
                        Summoner2.Add(hero.NetworkId, Texture.FromMemory(
                            Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(texture, typeof(byte[])),
                            13,
                            13, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0));
                    }
                }
            }
            #endregion
            #endregion
            #region SetText

            _abilityText = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Calibri",
                    Height = 13,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.ClearType
                });

            #endregion
        }

        public static void OnDraw()
        {
            #region return if Drawing Device == null or Drawings Disposed
            if (Drawing.Direct3DDevice == null || Drawing.Direct3DDevice.IsDisposed || _sprite.IsDisposed)
                return;
            #endregion

            _sprite.Begin();

            #region MenuSettings
            var showAllies = Program.TrackerMenu["showAllies"].Cast<CheckBox>().CurrentValue;
            var showEnemies = Program.TrackerMenu["showEnemies"].Cast<CheckBox>().CurrentValue;
            var showXp = Program.TrackerMenu["showXp"].Cast<CheckBox>().CurrentValue;
            var showTimer = Program.TrackerMenu["showTimer"].Cast<CheckBox>().CurrentValue;
            #endregion

            foreach (var hero in ObjectManager.Get<AIHeroClient>().Where(h => h.IsHPBarRendered))// && !h.IsMe))
            {
                //Return if Ally or Enemy tracking isnt enabled in menu
                if (hero.IsAlly && !showAllies || hero.IsEnemy && !showEnemies || hero.IsMe)
                    continue;

                #region Sprite

                //HUD Sprite
                _sprite.Draw(_hudTexture,
                    SharpDX.Color.White,
                    null,
                    new Vector3(-(hero.HPBarPosition.X + OffsetHudX), -(hero.HPBarPosition.Y + OffsetHudY), 0));

                //Summonor Sprites
                foreach (var summoner in Summoners)
                {
                    var spell = hero.Spellbook.GetSpell(summoner);
                    var cd = spell.CooldownExpires - Game.Time;
                    var spellPos = hero.GetSummoneroffset(spell.Slot);
                    var texture = spell.Slot == SpellSlot.Summoner1 ? Summoner1[hero.NetworkId] : Summoner2[hero.NetworkId];
                    _sprite.Draw(texture, new ColorBGRA(255, 255, 255, 255), null, new Vector3(-spellPos.X, -spellPos.Y, 0));
                    if (cd > 0)
                    {
                        _sprite.Draw(_summonerCd, new ColorBGRA(255, 255, 255, 255), null, new Vector3(-spellPos.X, -spellPos.Y, 0));
                    }
                }

                //Ability Sprites
                foreach (var ability in Abilities)
                {
                    if (hero.Spellbook.CanUseSpell(ability) != SpellState.NotLearned)
                    {
                        var cd = hero.Spellbook.GetSpell(ability).CooldownExpires - Game.Time;
                        var spellPos = hero.GetSpelloffset(ability);
                        var percent = (cd > 0 && Math.Abs(hero.Spellbook.GetSpell(ability).Cooldown) > float.Epsilon)
                            ? 1f - (cd / hero.Spellbook.GetSpell(ability).Cooldown)
                            : 1f;

                        _sprite.Draw(cd > 0 ? _onCd : _isReady, new ColorBGRA(255, 255, 255, 255),
                            new SharpDX.Rectangle(0, 0, (int)(percent * 22), 9),
                            new Vector3(-spellPos.X, -spellPos.Y, 0));
                    }
                }

                //XP Sprite (xp bar)
                if (showXp)
                {
                    _sprite.Draw(_xpBar, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 0, (int)(104 * (hero.Experience.XPPercentage/100)), 3),
                        new Vector3(-(hero.HPBarPosition.X - OffsetXpX), -(hero.HPBarPosition.Y - OffsetXpY), 0));
                }

                //Sprite.End();
                #endregion
                #region Text
                //CoolDown Timers
                if (showTimer)
                {
                    //Ability Timers
                    foreach (var ability in Abilities)
                    {
                        if (hero.Spellbook.CanUseSpell(ability) != SpellState.NotLearned)
                        {
                            var cd = hero.Spellbook.GetSpell(ability).CooldownExpires - Game.Time;
                            var spellPos = hero.GetSpelloffset(ability);
                            if (cd > 0)
                            {
                                ColorBGRA color = SharpDX.Color.AntiqueWhite;
                                var cdFrom = cd < 1 ? cd.ToString("0.0") : cd.ToString("0");
                                _abilityText.DrawText(null, cdFrom, (int)spellPos.X + 10 - cdFrom.Length * 2,
                                    (int)spellPos.Y + 12,
                                    color);
                            }
                        }
                    }

                    //Summoner Timers
                    foreach (var summoner in Summoners)
                    {
                        var cd = hero.Spellbook.GetSpell(summoner).CooldownExpires - Game.Time;
                        var spellPos = hero.GetSummoneroffset(summoner);
                        if (cd > 0)
                        {
                            ColorBGRA color = SharpDX.Color.AntiqueWhite;
                            var cdFrom = cd < 1 ? cd.ToString("0.0") : cd.ToString("0");
                            _abilityText.DrawText(null, cdFrom, (int)spellPos.X - 27 + cdFrom.Length, (int)spellPos.Y - 1,
                                color);
                        }
                    }
                }
                #endregion
            }

            _sprite.End();
        }

        public static void DomainUnload()
        {
            _abilityText.Dispose();
            _sprite.Dispose();
        }

        public static void OnPostReset()
        {
            _abilityText.OnResetDevice();
            _sprite.OnResetDevice();
        }

        public static void OnPreReset()
        {
            _abilityText.OnLostDevice();
            _sprite.OnLostDevice();
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

        private static Bitmap GetSummonerSprite(this SpellDataInst spell)
        {
            switch (spell.Name)
            {
                case "itemsmiteaoe":
                    return Resources.itemsmiteaoe;
                case "s5_summonersmiteduel":
                    return Resources.s5_summonersmiteduel;
                case "s5_summonersmiteplayerganker":
                    return Resources.s5_summonersmiteplayerganker;
                case "s5_summonersmitequick":
                    return Resources.s5_summonersmitequick;
                case "summonerbarrier":
                    return Resources.summonerbarrier;
                case "summonerboost":
                    return Resources.summonerboost;
                case "summonerclairvoyance":
                    return Resources.summonerclairvoyance;
                case "summonerdot":
                    return Resources.summonerdot;
                case "summonerexhaust":
                    return Resources.summonerexhaust;
                case "summonerflash":
                    return Resources.summonerflash;
                case "summonerhaste":
                    return Resources.summonerhaste;
                case "summonerheal":
                    return Resources.summonerheal;
                case "summonermana":
                    return Resources.summonermana;
                case "summonerodinGarrison":
                    return Resources.summonerodingarrison;
                case "summonerrevive":
                    return Resources.summonerrevive;
                case "summonersmite":
                    return Resources.summonersmite;
                case "summonerteleport":
                    return Resources.summonerteleport;
            }

            return Resources.summonerdot;
        }
    }
}
