using StalkerMUD.Common;

namespace StalkerMUD.Server.Entities
{
    public class Attributes
    {
        public int Id { get; set; }

        public Dictionary<AttributeType, int> Data { get; set; }
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
