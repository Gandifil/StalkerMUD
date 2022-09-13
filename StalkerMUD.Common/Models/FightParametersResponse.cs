namespace StalkerMUD.Common.Models
{
    public class FightParametersResponse
    {
        public string Name { get; set; }

        public Dictionary<AttributeType, int> Attributes { get; set; } = new();

        public int MaxHP { get; set; }

        public int CritPercent { get; set; }

        public int Resistance { get; set; }

        public float CritFactor { get; set; }
    }
}
