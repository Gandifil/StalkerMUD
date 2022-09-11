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

        public Dictionary<AttributeType, int> Attributes { get; set; }
        = new Dictionary<AttributeType, int>
        {
            { AttributeType.Health, 5 },
            { AttributeType.WeaponLevel, 5 },
            { AttributeType.Accuracy, 5 },
            { AttributeType.Dodge, 5 },
            { AttributeType.WeakExploit, 5 },
            { AttributeType.Speed, 5 },
        };
    }
}
