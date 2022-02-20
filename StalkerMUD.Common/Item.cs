using StalkerMUD.Common.Items;

namespace StalkerMUD.Common
{
    public abstract class Item
    {
        public abstract string Name { get; }

        public static readonly List<Item> Items = new List<Item>
        {
            new Shotgun(),
        };
    }
}
