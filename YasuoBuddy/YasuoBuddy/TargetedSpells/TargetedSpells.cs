using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;

namespace YasuoBuddy.TargetedSpells
{
        public static class TargetSpellDatabase
        {
            public static List<TargetSpellData> Spells;

            static TargetSpellDatabase()
            {
                Spells = new List<TargetSpellData>
            {

                #region Akali
                new TargetSpellData("akali", "akalimota", SpellSlot.Q, SpellType.Targeted, CcType.No, 600, 650, 1000),
                #endregion Akali

                #region Anivia
                new TargetSpellData("anivia", "frostbite", SpellSlot.E, SpellType.Targeted, CcType.No, 650, 500, 1200),
                #endregion Anivia

                #region Annie
                new TargetSpellData("annie", "disintegrate", SpellSlot.Q, SpellType.Targeted, CcType.No, 623, 500, 1400),
                #endregion Annie

                #region Caitlyn
                new TargetSpellData("caitlyn", "caitlynaceinthehole", SpellSlot.R, SpellType.Targeted, CcType.No, 2500, 0, 1500),
                #endregion Caitlyn

                #region Elise
                new TargetSpellData("elise", "elisehumanq", SpellSlot.Q, SpellType.Targeted, CcType.No, 625, 750, 2200),
                #endregion Elise
                
                #region FiddleSticks
                new TargetSpellData("fiddlesticks", "drain", SpellSlot.W, SpellType.Targeted, CcType.No, 575, 500, float.MaxValue),
                new TargetSpellData("fiddlesticks", "fiddlesticksdarkwind", SpellSlot.E, SpellType.Targeted, CcType.Silence, 750, 500, 1100),
                #endregion FiddleSticks

                #region Gangplank
                new TargetSpellData("gangplank", "parley", SpellSlot.Q, SpellType.Targeted, CcType.No, 625, 500, 2000),
                #endregion Gangplank

                #region Kassadin
                new TargetSpellData("kassadin", "nulllance", SpellSlot.Q, SpellType.Targeted, CcType.Silence, 650, 500, 1400),
                #endregion Kassadin

                #region Katarina
                new TargetSpellData("katarina", "katarinaq", SpellSlot.Q, SpellType.Targeted, CcType.No, 675, 500, 1800),
                #endregion Katarina

                #region Kayle
                new TargetSpellData("kayle", "judicatorreckoning", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 650, 500, 1500),
                #endregion Kayle

                #region Leblanc
                new TargetSpellData("leblanc", "leblancchaosorb", SpellSlot.Q, SpellType.Targeted, CcType.No, 700, 500, 2000),
                new TargetSpellData("leblanc", "leblancchaosorbm", SpellSlot.R, SpellType.Targeted, CcType.No, 700, 500, 2000),
                #endregion Leblanc

                #region Lucian
                new TargetSpellData("lucian", "lucianq", SpellSlot.Q, SpellType.Targeted, CcType.No, 550, 500, 500),
                #endregion Lucian

                #region Malphite
                new TargetSpellData("malphite", "seismicshard", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 625, 500, 1200),
                #endregion Malphite

                #region MissFortune
                new TargetSpellData("missfortune", "missfortunericochetshot", SpellSlot.Q, SpellType.Targeted, CcType.No, 650, 500, 1400),
                #endregion MissFortune

                #region Nami
                new TargetSpellData("nami", "namie", SpellSlot.E, SpellType.Targeted, CcType.Slow, 800, 500, float.MaxValue),
                #endregion Nami

                #region Nunu
                new TargetSpellData("nunu", "iceblast", SpellSlot.E, SpellType.Targeted, CcType.Slow, 550, 500, 1000),
                #endregion Nunu

                #region Pantheon
                new TargetSpellData("pantheon", "pantheonq", SpellSlot.Q, SpellType.Targeted, CcType.No, 600, 500, 1500),
                #endregion Pantheon

                #region Shaco
                new TargetSpellData("shaco", "twoshivpoisen", SpellSlot.E, SpellType.Targeted, CcType.Slow, 625, 500, 1500),
                #endregion Shaco

                #region Shen
                new TargetSpellData("shen", "shenvorpalstar", SpellSlot.Q, SpellType.Targeted, CcType.No, 475, 500, 1500),
                #endregion Shen

                #region Taric
                new TargetSpellData("taric", "dazzle", SpellSlot.E, SpellType.Targeted, CcType.Stun, 625, 500, 1400),
                #endregion Taric

                #region Teemo
                new TargetSpellData("teemo", "blindingdart", SpellSlot.Q, SpellType.Targeted, CcType.Blind, 580, 500, 1500),
                #endregion Teemo

                #region Tristana
                new TargetSpellData("tristana", "detonatingshot", SpellSlot.E, SpellType.Targeted, CcType.No, 625, 500, 1400),
                new TargetSpellData("tristana", "bustershot", SpellSlot.R, SpellType.Targeted, CcType.Knockback, 700, 500, 1600),
                #endregion Tristana

                #region Urgot
                new TargetSpellData("urgot", "urgotheatseekingmissile", SpellSlot.Q, SpellType.Targeted, CcType.No, 1000, 500, 1600),
                #endregion Urgot

                #region Vayne
                new TargetSpellData("vayne", "vaynecondemm", SpellSlot.E, SpellType.Targeted, CcType.Stun, 450, 500, 1200),
                #endregion Vayne

                #region Vladimir
                new TargetSpellData("vladimir", "vladimirtransfusion", SpellSlot.Q, SpellType.Targeted, CcType.No, 600, 500, 1400),
                #endregion Vladimir

            };
            }

            public static TargetSpellData GetByName(string spellName)
            {
                spellName = spellName.ToLower();
                return Spells.FirstOrDefault(spell => spell.Name == spellName);
            }
        }
}
