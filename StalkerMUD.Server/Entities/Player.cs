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

        public int MaxHP => 10 * Attributes[AttributeType.Health];

        public int CritPercent => Attributes[AttributeType.WeakExploit] * 2;

        public float CritFactor => 2.0f + 0.1f * Attributes[AttributeType.WeakExploit];
    }
}
