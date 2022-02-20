using StalkerMUD.Client.UI;
using StalkerMUD.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Client.Screens
{
    internal class CharacterView : Screen
    {
        public override string Name => "Персонаж";

        public override string Description => "";

        private readonly Game _game;

        private readonly Screen _backScreen;

        public CharacterView(Game game, Screen backScreen)
        {
            _game = game;
            _backScreen = backScreen;
        }

        protected override void Render()
        {
            Console.WriteLine($"Имя: {_game.Player.Name}");
            foreach (Player.AttributeType attribute in (Player.AttributeType[])Enum.GetValues(typeof(Player.AttributeType)))
            {
                Console.WriteLine($"{Player.AttributeNames[attribute]}: {_game.Player.Attributes[attribute]}");
            }
        }

        public override ChoiceBox GenerateChoices()
        {
            return new ChoiceBox(new ChoiceBox.Case("Изменить хар-ки") { Screen = new UpgradeCharacter(_game, this)})
            {
                BackScreen = _backScreen,
            };
        }
    }
}
