using StalkerMUD.Client.UI;
using StalkerMUD.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Client.Screens
{
    internal class UpgradeCharacter : Screen
    {
        public override string Name => "Распределение очков характеристик";

        public override string Description => Attributes.Names[CurrentAttribute];

        public AttributeType CurrentAttribute { get; set; } = AttributeType.Health;

        private readonly Game _game;

        private readonly Screen _backScreen;

        public UpgradeCharacter(Game game, Screen backScreen)
        {
            _game = game;
            _backScreen = backScreen;
        }

        protected override void Render()
        {
            Console.WriteLine($"Свободных очков: {_game.Player.AttributeFreePoints}");
            Console.WriteLine($"Выбранная характеристика: {Attributes.Names[CurrentAttribute]}");
        }

        public override ChoiceBox GenerateChoices()
        {
            var cases = ((AttributeType[])Enum.GetValues(typeof(AttributeType)))
                .Select(x => new ChoiceBox.Case($"{Attributes.Names[x]} - {_game.Player.Attributes[x]}")
                {
                    Action = () => { CurrentAttribute = x; },
                    Color = x == CurrentAttribute ? ConsoleColor.Green : ConsoleColor.Gray,
                });
            return new ChoiceBox(cases.ToArray())
            {
                BackScreen = _backScreen,
                EnterCase = new ChoiceBox.Case("Увеличить")
                {
                    Action = UpgradeAttribute,
                }
            };
        }

        private void UpgradeAttribute()
        {
            var player = _game.Player;

            if (player.AttributeFreePoints > 0)
            { 
                player.AttributeFreePoints--;
                player.Attributes[CurrentAttribute]++;
            }
        }
    }
}
