using LiteDB;

namespace StalkerMUD.Server.Entities
{
    public class Mob
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [BsonRef]
        public Attributes Attributes { get; set; } = new();
    }
}
