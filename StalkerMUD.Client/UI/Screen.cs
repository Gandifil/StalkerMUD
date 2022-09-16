namespace StalkerMUD.Client.UI
{
    internal interface IScreen
    {
        Task Show();
    }

    internal abstract class Screen : IScreen
    {
        public abstract string Name { get; }

        public virtual string Description { get; } = string.Empty;

        public virtual async Task Show()
        {
            Console.Clear();

            if (!string.IsNullOrEmpty(Name))
                Console.WriteLine(Name.ToUpper());

            if (!string.IsNullOrEmpty(Description))
                Console.WriteLine(Description);
        }
    }
}
