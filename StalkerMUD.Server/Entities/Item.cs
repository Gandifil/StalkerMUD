using StalkerMUD.Common;

namespace StalkerMUD.Server.Entities
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ItemType Type { get; set; }

        public int Health { get; set; }

        public int Resistance { get; set; }

        public int Damage { get; set; }

        public int Rounds { get; set; }
    }
}
