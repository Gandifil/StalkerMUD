namespace StalkerMUD.Client.UI
{
    internal class ErrorScreen : IScreen
    {
        private readonly string _message;

        public ErrorScreen(string message)
        {
            _message = message;
        }

        public async Task Show()
        {
            var foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(_message);
            Console.ForegroundColor = foregroundColor;
            Console.ReadKey();
        }
    }
}
