using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using SharpDX;
using Color = System.Drawing.Color;

namespace RivenBuddy
{
    namespace DamageIndicator
    {
        public class SpellData
        {
            private readonly DamageType _damageType = DamageType.Physical;
            private readonly int _flatDamage = -1;
            private readonly bool _onlyIfActive;
            private readonly SpellSlot _spellSlot = SpellSlot.Unknown;
            private readonly DamageLibrary.SpellStages _stage = DamageLibrary.SpellStages.Default;
            public Color Color = Color.FromArgb(100, Color.Aqua);

            public SpellData(int flatDamage, DamageType damageType, Color c)
            {
                _flatDamage = flatDamage;
                _damageType = damageType;
            }

            public SpellData(SpellSlot spellSlot, bool checkIfActive, DamageLibrary.SpellStages spellStage, Color c)
            {
                _onlyIfActive = checkIfActive;
                _spellSlot = spellSlot;
                _stage = spellStage;
                Color = c;
            }

            public void SetColor(Color c)
            {
                Color = c;
            }

            public float CalculateDamage(AIHeroClient target, AIHeroClient source = null)
            {
                if (source == null) source = Player.Instance;
                if (_spellSlot != SpellSlot.Unknown &&
                    (!_onlyIfActive || _onlyIfActive && source.Spellbook.GetSpell(_spellSlot).State == SpellState.Ready))
                {
                    return source.GetSpellDamage(target, _spellSlot, _stage);
                }
                if (_flatDamage != -1)
                {
                    return source.CalculateDamageOnUnit(target, _damageType, _flatDamage);
                }
                return 0;
            }
        }

        public class DamageIndicator
        {
            private readonly float _barLength = 104;
            private readonly float _xOffset = -9;
            private readonly float _yOffset = +22;
            private Dictionary<string, SpellData> _damageItems = new Dictionary<string, SpellData>();
            public float CheckDistance = 1200;

            public DamageIndicator()
            {
                Drawing.OnEndScene += Drawing_OnDraw;
                //Chat.Print("Damage Indicator By Fluxy - Loaded", Color.Red);
            }

            public void Add(string s, SpellData data)
            {
                if (_damageItems.ContainsKey(s))
                {
                    Logger.Log(LogLevel.Error,
                        "Damage Indicator: Cannot Add '{0}', already added. Please Use Update to change the values.", s);
                    return;
                }
                _damageItems.Add(s, data);
            }

            public void Update(string s, SpellData data)
            {
                if (_damageItems.ContainsKey(s))
                {
                    _damageItems[s] = data;
                }
                else
                {
                    Logger.Log(LogLevel.Error, "Damage Indicator: No Item of '{0}' is not found, cannot update", s);
                }
            }

            public void Reset()
            {
                _damageItems = new Dictionary<string, SpellData>();
            }

            private void Drawing_OnDraw(EventArgs args)
            {
                if (!Program.DrawMenu["draw.Damage"].Cast<CheckBox>().CurrentValue) return;

                foreach (var aiHeroClient in EntityManager.Heroes.Enemies)
                {
                    if (!aiHeroClient.IsHPBarRendered) continue;

                    var pos = new Vector2(aiHeroClient.HPBarPosition.X + _xOffset,
                        aiHeroClient.HPBarPosition.Y + _yOffset);
                    
                        var fullbar = (_barLength)*(aiHeroClient.HealthPercent/100);
                        var damage = (_barLength)*
                                     ((DamageHandler.ComboDamage(aiHeroClient)/aiHeroClient.MaxHealth) > 1
                                         ? 1
                                         : (DamageHandler.ComboDamage(aiHeroClient) /aiHeroClient.MaxHealth));
                        Line.DrawLine(Color.FromArgb(100, Color.Aqua), 9f, new Vector2(pos.X, pos.Y),
                            new Vector2((float) (pos.X + (damage > fullbar ? fullbar : damage)), pos.Y));
                        Line.DrawLine(Color.Black, 3, new Vector2((float) (pos.X + (damage > fullbar ? fullbar : damage) + 1), pos.Y - 4), new Vector2((float) (pos.X + (damage > fullbar ? fullbar : damage) + 1), pos.Y + 5));
                }
            }
        }
    }
}