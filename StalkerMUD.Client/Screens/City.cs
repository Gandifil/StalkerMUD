using StalkerMUD.Client.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Client.Screens
{
    internal class City : Screen
    {
        public override string Name => "Город";

        public override string Description => "";

        private readonly Game _game;

        private readonly Shop _shop;

        public City(Game game)
        {
            _game = game;
            _shop = new Shop(_game, this);
        }

        public override ChoiceBox GenerateChoices()
        {
            return new ChoiceBox(new List<ChoiceBox.Case>{
                new ChoiceBox.Case("Арена")
                {
                    Screen = _shop,
                },
                new ChoiceBox.Case("Персонаж")
                {
                    Screen = new CharacterView(_game, this),
                },
                new ChoiceBox.Case("Торговец")
                {
                    Screen = _shop,
                },
            });
        }
    }
}
