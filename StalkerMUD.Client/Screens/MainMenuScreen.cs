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

        public override ChoiceBox GenerateChoices()
        {
            return new ChoiceBox(new List<ChoiceBox.Case>{
                new ChoiceBox.Case("Зарегистрироваться")
                {
                    Screen = new RegistrationScreen(),
                },
                new ChoiceBox.Case("Торговец")
                {
                    Screen = null,
                },
            });
        }
    }
}
