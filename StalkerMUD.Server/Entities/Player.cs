namespace StalkerMUD.Server.Entities
{
    public class Player
    {
        public class ItemPack
        {
            public int Id;
            public int Count;
        }

        public Dictionary<int, ItemPack> Items { get; set; } = new();

        public int Money { get; set; } = 2500;

        public void AddItem(int itemId)
        {
            if (Items.TryGetValue(itemId, out var item))
                item.Count++;
            else
                Items.Add(itemId, new ItemPack { Id = itemId, Count = 1 });
        }
    }
}
