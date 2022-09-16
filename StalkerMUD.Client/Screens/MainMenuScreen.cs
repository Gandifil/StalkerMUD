using StalkerMUD.Client.Screens.Auth;
using StalkerMUD.Client.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Client.Screens
{
    internal class MainMenuScreen : Screen
    {
        public override string Name => string.Empty;

        private readonly ScreenPlayer _screenPlayer;

        public MainMenuScreen(ScreenPlayer screenPlayer)
        {
            _screenPlayer = screenPlayer;
        }

        public override async Task Show()
        {
            await base.Show();

            var choices = new ChoiceBox(new List<ChoiceBox.Case>{
                new ChoiceBox.Case(() => _screenPlayer.AddNextScreen<RegistrationScreen>(), "Зарегистрироваться"),
                new ChoiceBox.Case(() => _screenPlayer.AddNextScreen<LoginScreen>(), "Войти"),
            });

            choices.Show();
        }
    }
}
