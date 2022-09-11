namespace StalkerMUD.Common.Models
{
    public class PlayerResponse
    {
        public string Name { get; set; }

        public int AttributeFreePoints { get; set; }

        public Dictionary<AttributeType, int> Attributes { get; set; } = new();

        public int MaxHP { get; set; }

        public int CritPercent { get; set; }

        public float CritFactor { get; set; }
    }
}
