using StalkerMUD.Common.Items;

namespace StalkerMUD.Common
{
    public abstract class Item
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public static readonly List<Item> Items = new List<Item>
        {
            new Shotgun(),
        };
    }
}
