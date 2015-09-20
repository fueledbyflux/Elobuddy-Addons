using EloBuddy;

namespace ActivatorBuddy.Items
{
    public enum CastType
    {
        Targeted,
        Skillshot,
        SelfCast
    }

    public enum ItemType
    {
        Offensive,
        Defensive,
        Healing,
        ManaRestore,
        Cleanse
    }

    public class Item
    {
        public string BuffName;
        public CastType CastType;
        public ItemId Id;
        public ItemType ItemType;
        public bool MeleeOnly;
        public string Name;
        public int Range;

        public Item(string name, int range, CastType castType, ItemId id, ItemType itemType, bool meleeOnly = false,
            string buffName = null)
        {
            Name = name;
            Range = range;
            CastType = castType;
            Id = id;
            ItemType = itemType;
            MeleeOnly = meleeOnly;
            BuffName = buffName;
        }
    }
}