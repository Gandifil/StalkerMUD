using Microsoft.Extensions.DependencyInjection;

namespace StalkerMUD.Client.UI
{
    internal class ScreenPlayer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Queue<Type> _screens = new();
        private IScreen? _current;

        public ScreenPlayer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Restart()
        {
            _screens.Enqueue(_current?.GetType() ?? throw new NullReferenceException(nameof(_current)));
        }

        public void AddNextScreen<T>() where T : Screen
        {
            _screens.Enqueue(typeof(T));
        }

        public void StartShowCycle()
        {
            while (true)
            {
                var screenType = _screens.Dequeue();
                ShowScreen(screenType);
            }
        }

        private void ShowScreen(Type type)
        {
            using var scope = _serviceProvider.CreateScope();
            _current = (scope.ServiceProvider.GetRequiredService(type) as IScreen) ?? throw new ArgumentNullException();
            _current.Show();
        }
    }
}
