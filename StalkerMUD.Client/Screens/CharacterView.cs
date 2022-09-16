using StalkerMUD.Client.UI;
using StalkerMUD.Common;

namespace StalkerMUD.Client.Screens
{
    internal class CharacterView : Screen
    {
        public override string Name => "Персонаж";

        private readonly IPlayerClient _playerClient;
        private readonly ScreenPlayer _screenPlayer;

        public CharacterView(IPlayerClient playerClient, ScreenPlayer screenPlayer)
        {
            _playerClient = playerClient;
            _screenPlayer = screenPlayer;
        }

        public override async Task Show()
        {
            await base.Show();

            var player = await _playerClient.PlayerAsync();

            Console.WriteLine($"Имя: {player.Name}");
            foreach (AttributeType attribute in (AttributeType[])Enum.GetValues(typeof(AttributeType)))
            {
                Console.WriteLine($"{Attributes.Names[attribute]}: {player.Attributes[attribute]}");
            }
            Console.WriteLine($"HP: {player.MaxHP}");
            Console.WriteLine($"Защита: {player.Resistance}");
            Console.WriteLine($"Шанс критического урона: {player.CritPercent}%");
            Console.WriteLine($"Количество критического урона: {player.CritFactor * 100}%");

            GenerateChoices().Show();
        }

        private ChoiceBox GenerateChoices()
        {
            return new ChoiceBox(new ChoiceBox.Case(() => _screenPlayer.AddNextScreen<UpgradeCharacter>(), 
                "Изменить характеристики"))
            {
                BackCase = new ChoiceBox.Case(() => _screenPlayer.AddNextScreen<City>())
            };
        }
    }
}
