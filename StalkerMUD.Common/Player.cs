namespace StalkerMUD.Common
{
    public class Player
    {
        public string Name { get; set; }

        public int Money { get; set; } = 15000;

        public int AttributeFreePoints { get; set; } = 10;

        public List<Item> Items { get; } = new List<Item>();

        public Dictionary<AttributeType, int> Attributes { get; }
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
