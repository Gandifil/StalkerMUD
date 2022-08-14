using Microsoft.Extensions.DependencyInjection;

namespace StalkerMUD.Client.UI
{
    internal class ScreenPlayer
    {
        private readonly IServiceProvider _serviceProvider;

        public ScreenPlayer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void StartShowCycle<T>() where T: Screen
        {
            Screen startScreen = _serviceProvider.GetRequiredService<T>() ?? throw new ArgumentNullException();
            var screen = startScreen;
            while (true)
                screen = screen.Show() ?? screen;
        }
    }
}
