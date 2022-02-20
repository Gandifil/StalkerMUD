namespace StalkerMUD.Common
{
    public class Player
    {
        public string Name { get; set; }

        public int Money { get; set; } = 15000;

        public int AttributeFreePoints { get; set; } = 10;

        public List<Item> Items { get; } = new List<Item>();

        public enum AttributeType
        {
            Health,
            WeaponLevel,
            Accuracy,
            Dodge,
            WeakExploit,
            Speed
        }

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

        public static Dictionary<AttributeType, string> AttributeNames { get; }
        = new ()
        {
            { AttributeType.Health, "Здоровье" },
            { AttributeType.WeaponLevel, "Владение оружием" },
            { AttributeType.Accuracy, "Точность" },
            { AttributeType.Dodge, "Уклонение" },
            { AttributeType.WeakExploit, "Знание противников" },
            { AttributeType.Speed, "Скорость" },
        };
    }
}
