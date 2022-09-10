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

        public override string Description => string.Empty;

        private readonly ScreenPlayer _screenPlayer;

        public MainMenuScreen(ScreenPlayer screenPlayer)
        {
            _screenPlayer = screenPlayer;
        }

        public override void Show()
        {
            base.Show();

            var choices = new ChoiceBox(new List<ChoiceBox.Case>{
                new ChoiceBox.Case("Зарегистрироваться")
                {
                    Action = () => _screenPlayer.AddNextScreen<RegistrationScreen>(),
                },
                new ChoiceBox.Case("Войти")
                {
                    Action = () => _screenPlayer.AddNextScreen<LoginScreen>(),
                },
            });

            choices.Show();
        }
    }
}
