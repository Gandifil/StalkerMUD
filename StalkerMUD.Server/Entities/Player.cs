using LiteDB;
using StalkerMUD.Common;

namespace StalkerMUD.Server.Entities
{
    public class Player
    {
        public class ItemPack
        {
            public int Id { get; set; }
            public int Count { get; set; }
        }

        public Dictionary<int, ItemPack> Items { get; set; } = new();

        public int Money { get; set; } = 2500;

        public int AttributeFreePoints { get; set; } = 10;

        public int? SelectedWeaponId { get; set; }

        public int? SelectedSuitId { get; set; }

        public void AddItem(int itemId)
        {
            if (Items.TryGetValue(itemId, out var item))
                item.Count++;
            else
                Items.Add(itemId, new ItemPack { Id = itemId, Count = 1 });
        }

        public Attributes Attributes { get; set; } = new();
    }
}
