namespace StalkerMUD.Common.Models
{
    public class ActorChangeResponse
    {
        public enum ParameterType
        {
            Hp
        }

        public int Id { get; set; }

        public ParameterType Type { get; set; }

        public int Value { get; set; }
    }
}
