using System;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using Color = System.Drawing.Color;

namespace YasuoBuddy.EvadePlus.SkillshotTypes
{
    public class LinearMissileSkillshot : EvadeSkillshot
    {
        public LinearMissileSkillshot()
        {
            Caster = null;
            CastArgs = null;
            SpawnObject = null;
            SData = null;
            SpellData = null;
            Team = GameObjectTeam.Unknown;
            IsValid = true;
            TimeDetected = Environment.TickCount;
        }

        public Vector3 OverrideCastEndPos;
        public Vector3 OverrideCastStartPos;

        public MissileClient Missile
        {
            get { return SpawnObject as MissileClient; }
        }

        public Vector3 StartPosition
        {
            get
            {
                if (Missile == null)
                {
                    return OverrideCastStartPos != Vector3.Zero ? OverrideCastStartPos : Caster.Position;
                }
                else
                {
                    return Missile.Position;
                }
            }
        }

        public Vector3 EndPosition
        {
            get
            {
                if (Missile == null)
                {
                    return StartPosition.ExtendVector3(CastArgs == null ? OverrideCastEndPos : CastArgs.End,
                        SpellData.Range);
                }
                else
                {
                    return Missile.StartPosition.ExtendVector3(Missile.EndPosition, SpellData.Range);
                        //Missile.EndPosition;
                }
            }
        }

        public override Vector3 GetPosition()
        {
            return StartPosition;
        }

        public override EvadeSkillshot NewInstance()
        {
            var newInstance = new LinearMissileSkillshot() {SpellData = SpellData};
            return newInstance;
        }

        public override void OnCreateObject(GameObject obj)
        {
            var missile = obj as MissileClient;

            if (SpawnObject == null && missile != null)
            {
                if (missile.SData.Name == SpellData.MissileSpellName && missile.SpellCaster.Index == Caster.Index)
                {
                    // Force skillshot to be removed
                    IsValid = false;
                }
            }
        }

        public override void OnSpellDetection(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            //TODO: create new class
            if (SpellData.ExtraMissiles > 0)
            {
                OverrideCastStartPos = CastArgs.Start.ExtendVector3(EndPosition, Caster.BoundingRadius);

                var angle = (float) ((SpellData.Radius*5)/(2*Math.PI*SpellData.Range)*360);
                angle = (float) (angle/360*2*Math.PI);
                for (var i = 1; i <= SpellData.ExtraMissiles/2; i++)
                {
                    var skillshot = (LinearMissileSkillshot) NewInstance();
                    skillshot.SkillshotDetector = SkillshotDetector;
                    skillshot.Caster = sender;
                    skillshot.OverrideCastEndPos =
                        (StartPosition - EndPosition).To2D().Normalized().Rotated(angle*i).To3DWorld()*
                        -SpellData.Range + StartPosition;
                    skillshot.OverrideCastStartPos = CastArgs.Start.ExtendVector3(skillshot.OverrideCastEndPos,
                        Caster.BoundingRadius + i*6);
                    skillshot.SData = args.SData;
                    skillshot.Team = sender.Team;
                    SkillshotDetector.DetectedSkillshots.Add(skillshot);

                    skillshot = (LinearMissileSkillshot) NewInstance();
                    skillshot.SkillshotDetector = SkillshotDetector;
                    skillshot.Caster = sender;
                    skillshot.OverrideCastEndPos =
                        (StartPosition - EndPosition).To2D().Normalized().Rotated(angle*-i).To3DWorld()*-SpellData.Range +
                        StartPosition;
                    skillshot.OverrideCastStartPos = CastArgs.Start.ExtendVector3(skillshot.OverrideCastEndPos,
                        Caster.BoundingRadius + i*6);
                    skillshot.SData = args.SData;
                    skillshot.Team = sender.Team;
                    SkillshotDetector.DetectedSkillshots.Add(skillshot);
                }
            }
        }

        public override void OnTick()
        {
            if (Missile == null)
            {
                if (Environment.TickCount > TimeDetected + SpellData.Delay + 50)
                    IsValid = false;
            }
            else
            {
                if (Environment.TickCount > TimeDetected + 9000)
                    IsValid = false;
            }
        }

        public override void OnDraw()
        {
            if (!IsValid)
            {
                return;
            }

            Utils.Draw3DRect(StartPosition, EndPosition, SpellData.Radius*2, Color.White);
        }

        public override Geometry.Polygon ToPolygon(float extrawidth = 0)
        {
            if (SpellData.AddHitbox)
                extrawidth += Player.Instance.BoundingRadius/2;

            return new Geometry.Polygon.Rectangle(StartPosition, EndPosition.ExtendVector3(StartPosition, -extrawidth),
                SpellData.Radius*2 + extrawidth);
        }

        public override int GetAvailableTime(AIHeroClient hero = null)
        {
            hero = hero ?? Player.Instance;

            var dist1 =
                Math.Abs((EndPosition.Y - StartPosition.Y)*hero.ServerPosition.X -
                         (EndPosition.X - StartPosition.X)*hero.ServerPosition.Y +
                         EndPosition.X*StartPosition.Y - EndPosition.Y*StartPosition.X)/
                (StartPosition.Distance(EndPosition));

            var actualDist =
                Math.Sqrt(StartPosition.Distance(hero.ServerPosition).Pow() - dist1.Pow());

            var time = SpellData.MissileSpeed > 0 ? (int) ((actualDist/SpellData.MissileSpeed)*1000) : 0;

            if (Missile == null)
            {
                time += Math.Max(0, SpellData.Delay - (Environment.TickCount - TimeDetected));
            }

            return time;
        }

        public override bool IsFromFow()
        {
            return Missile != null && !Missile.SpellCaster.IsVisible;
        }
    }
}