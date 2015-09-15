using EloBuddy;

namespace VayneBuddy.Activator
{
    public enum CastType { Targeted, Skillshot, SelfCast }
    public enum ItemType { Offensive, Defensive, Healing, ManaRestore, Cleanse }
    public class Item
    {
        public string Name;
        public int Range;
        public CastType CastType;
        public ItemId Id;
        public bool MeleeOnly;
        public ItemType ItemType;
        public string BuffName;

        public Item(string name, int range, CastType castType, ItemId id, ItemType itemType, bool meleeOnly = false, string buffName = null)
        {
            Name = name;
            Range = range;
            CastType = castType;
            Id = id;
            ItemType = itemType;
            MeleeOnly = meleeOnly;
            BuffName = buffName;
        }

        // TODO; finish up the skillshot shit :P
    }
}
