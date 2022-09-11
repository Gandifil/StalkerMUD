using StalkerMUD.Client.UI;
using StalkerMUD.Common;

namespace StalkerMUD.Client.Screens
{
    internal class CharacterView : Screen
    {
        public override string Name => "Персонаж";

        public override string Description => "";

        private readonly PlayerClient _playerClient;
        private readonly ScreenPlayer _screenPlayer;

        public CharacterView(PlayerClient playerClient, ScreenPlayer screenPlayer)
        {
            _playerClient = playerClient;
            _screenPlayer = screenPlayer;
        }

        public override void Show()
        {
            base.Show();

            var player = _playerClient.PlayerAsync().Result;

            Console.WriteLine($"Имя: {player.Name}");
            foreach (AttributeType attribute in (AttributeType[])Enum.GetValues(typeof(AttributeType)))
            {
                Console.WriteLine($"{Attributes.Names[attribute]}: {player.Attributes[attribute]}");
            }
            Console.WriteLine($"HP: {player.MaxHP}");
            Console.WriteLine($"Шанс критического урона: {player.CritPercent}%");
            Console.WriteLine($"Количество критического урона: {player.CritFactor * 100}%");

            GenerateChoices().Show();
        }

        public override ChoiceBox GenerateChoices()
        {
            return new ChoiceBox(
                new ChoiceBox.Case("Изменить характеристики")
                {
                    Action = () => _screenPlayer.AddNextScreen<UpgradeCharacter>(),
                })
            {
                BackCase = new ChoiceBox.Case("")
                {
                    Action = () => _screenPlayer.AddNextScreen<City>(),
                }
            };
        }
    }
}
