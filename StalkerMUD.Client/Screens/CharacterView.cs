﻿using StalkerMUD.Client.UI;
using StalkerMUD.Common;

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
            foreach (AttributeType attribute in (AttributeType[])Enum.GetValues(typeof(AttributeType)))
            {
                Console.WriteLine($"{Attributes.Names[attribute]}: {_game.Player.Attributes[attribute]}");
            }
            Console.WriteLine($"HP: {_game.Player.MaxHP}");
            Console.WriteLine($"Шанс критического урона: {_game.Player.CritPercent}%");
            Console.WriteLine($"Количество критического урона: {_game.Player.CritFactor * 100}%");
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
