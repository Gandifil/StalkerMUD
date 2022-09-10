using StalkerMUD.Client.Logic;
using StalkerMUD.Client.UI;
using StalkerMUD.Common;

namespace StalkerMUD.Client.Screens.Auth
{
    internal abstract class BaseAuthScreen: Screen
    {
        private readonly ConnectionState _connectionState;
        private readonly ScreenPlayer _screenPlayer;

        public BaseAuthScreen(ConnectionState connectionState, ScreenPlayer screenPlayer)
        {
            _connectionState = connectionState;
            _screenPlayer = screenPlayer;
        }

        public override string Description => string.Empty;

        public override void Show()
        {
            base.Show();

            Console.Write("Логин: ");
            var login = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(login))
            {
                new ErrorScreen("Заполните логин").Show();
                _screenPlayer.Restart();
                return;
            }

            Console.Write("Пароль: ");
            var password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(login))
            {
                new ErrorScreen("Заполните пароль").Show();
                _screenPlayer.Restart();
                return;
            }

            try
            {
                _connectionState.Token = RequestToken(login, password);
                _screenPlayer.AddNextScreen<City>();
            }
            catch (Exception e)
            {
                new ErrorScreen("Ошибка: " + e.Message).Show();
                _screenPlayer.AddNextScreen<MainMenuScreen>();
            }
        }

        protected abstract string RequestToken(string login, string? password);
    }
}
