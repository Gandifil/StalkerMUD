namespace StalkerMUD.Server.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PasswordHash { get; set; }
    }
}
