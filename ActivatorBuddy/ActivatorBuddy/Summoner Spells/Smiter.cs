using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace ActivatorBuddy.Summoner_Spells
{
    class SmiteBarPosition
    {
        public SmiteBarPosition(Vector2 healthBarPos, int distance = 62, int height = 4)
        {
            HealthBarPos = healthBarPos;
            Distance = distance;
            Height = height;
        }
        public Vector2 HealthBarPos;
        public int Distance;
        public int Height;
    }
    class Smiter
    {
        public static readonly string[] SmiteableUnits =
        {
            "SRU_Red", "SRU_Blue", "SRU_Dragon", "SRU_Baron",
            "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak",
            "SRU_Krug",  "Sru_Crab"
        };

        private static readonly int[] SmitePurple = { 3713, 3726, 3725, 3724, 3723, 3933 };
        private static readonly int[] SmiteGrey = { 3711, 3722, 3721, 3720, 3719, 3932 };
        private static readonly int[] SmiteRed = { 3715, 3718, 3717, 3716, 3714, 3931 };
        private static readonly int[] SmiteBlue = { 3706, 3710, 3709, 3708, 3707, 3930 };

        public static void SetSmiteSlot()
        {
            SpellSlot smiteSlot;
            if (SmiteBlue.Any(x => ObjectManager.Player.InventoryItems.FirstOrDefault(a => a.Id == (ItemId) x) != null))
                smiteSlot = ObjectManager.Player.GetSpellSlotFromName("s5_summonersmiteplayerganker");
            else if (SmiteRed.Any(x => ObjectManager.Player.InventoryItems.FirstOrDefault(a => a.Id == (ItemId) x) != null))
                smiteSlot = ObjectManager.Player.GetSpellSlotFromName("s5_summonersmiteduel");
            else if (SmiteGrey.Any(x => ObjectManager.Player.InventoryItems.FirstOrDefault(a => a.Id == (ItemId) x) != null))
                smiteSlot = ObjectManager.Player.GetSpellSlotFromName("s5_summonersmitequick");
            else if (SmitePurple.Any(x => ObjectManager.Player.InventoryItems.FirstOrDefault(a => a.Id == (ItemId) x) != null))
                smiteSlot = ObjectManager.Player.GetSpellSlotFromName("itemsmiteaoe");
            else
                smiteSlot = ObjectManager.Player.GetSpellSlotFromName("summonersmite");
            SummonerSpells.Smite = new Spell.Targeted(smiteSlot, 500);
        }

        public static int GetSmiteDamage()
        {
            int level = ObjectManager.Player.Level;
            int[] smitedamage =
            {
                20*level + 370,
                30*level + 330,
                40*level + 240,
                50*level + 100
            };
            return smitedamage.Max();
        }

        /*
        public static void DrawHpBarLine(SmiteBarPosition s, Obj_AI_Base unit)
        {
            var smiteDmg = ObjectManager.Player.GetSummonerSpellDamage(unit, DamageLibrary.SummonerSpells.Smite);
            var bonusDistance = (smiteDmg/unit.MaxHealth)*s.Distance;
            Drawing.DrawLine(s.HealthBarPos.X + bonusDistance, s.HealthBarPos.Y, s.HealthBarPos.X + bonusDistance, s.HealthBarPos.Y + s.Height, 2, Color.White);
        }
        public static SmiteBarPosition GetSmiteBarPosition(Obj_AI_Base unit)
        {
            var barPos = unit.HPBarPosition;
            switch (unit.BaseSkinName)
            {
                case "SRU_Dragon":
                    return new SmiteBarPosition(new Vector2(barPos.X - 62, barPos.Y-7), 124);
                case "SRU_Baron":
                    return new SmiteBarPosition(new Vector2(barPos.X - 62, barPos.Y), 124);
                case "SRU_Red":
                case "SRU_Blue":
                    return new SmiteBarPosition(new Vector2(barPos.X - 72, barPos.Y), 144, 12);
                case "SRU_Murkwolf":
                case "SRU_Razorbeak":
                case "SRU_Krug":
                    return new SmiteBarPosition(new Vector2(barPos.X - 40, barPos.Y), 80);
            }
            return new SmiteBarPosition(unit.HPBarPosition);
        }
        */
    }
}
