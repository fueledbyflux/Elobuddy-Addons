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

        public class DamageIndicator
        {
            private readonly float _barLength = 104;
            private readonly float _xOffset = 2;
            private readonly float _yOffset = 9;
            public float CheckDistance = 1500;

            public DamageIndicator()
            {
                Drawing.OnEndScene += Drawing_OnDraw;
            }

            private void Drawing_OnDraw(EventArgs args)
            {
                if (!Program.DrawMenu["draw.Damage"].Cast<CheckBox>().CurrentValue) return;

                foreach (var aiHeroClient in EntityManager.Heroes.Enemies)
                {
                    if (!aiHeroClient.IsHPBarRendered || !aiHeroClient.VisibleOnScreen) continue;

                    var pos = new Vector2(aiHeroClient.HPBarPosition.X + _xOffset, aiHeroClient.HPBarPosition.Y + _yOffset);
                    var fullbar = (_barLength)*(aiHeroClient.HealthPercent/100);
                    var damage = (_barLength)*
                                     ((DamageHandler.ComboDamage(aiHeroClient)/aiHeroClient.MaxHealth) > 1
                                         ? 1
                                         : (DamageHandler.ComboDamage(aiHeroClient) /aiHeroClient.MaxHealth));
                        Line.DrawLine(Color.FromArgb(100, Color.Aqua), 9f, new Vector2(pos.X, pos.Y),
                            new Vector2((float) (pos.X + (damage > fullbar ? fullbar : damage)), pos.Y));
                        Line.DrawLine(Color.Black, 9, new Vector2((float) (pos.X + (damage > fullbar ? fullbar : damage) - 2), pos.Y), new Vector2((float) (pos.X + (damage > fullbar ? fullbar : damage) + 2), pos.Y));
                }
            }
        }
    }
}