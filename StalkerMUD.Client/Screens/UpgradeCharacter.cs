using StalkerMUD.Client.UI;
using StalkerMUD.Common;

namespace StalkerMUD.Client.Screens
{
    internal class UpgradeCharacter : Screen
    {
        public override string Name => "Распределение очков характеристик";

        private readonly IPlayerClient _playerClient;
        private readonly ScreenPlayer _screenPlayer;

        public UpgradeCharacter(IPlayerClient playerClient, ScreenPlayer screenPlayer)
        {
            _playerClient = playerClient;
            _screenPlayer = screenPlayer;
        }

        public override async Task Show()
        {
            await base.Show();

            var player = await _playerClient.PlayerAsync();

            Console.WriteLine($"Свободных очков: {player.AttributeFreePoints}");

            var cases = ((AttributeType[])Enum.GetValues(typeof(AttributeType)))
                .Select(x => new ChoiceBox.Case()
                {
                    Name = $"{Attributes.Names[x]} - {player.Attributes[x]}",
                    Action = () => { UpgradeAsync(x).Wait(); _screenPlayer.Restart(); },
                    IsEnabled = player.AttributeFreePoints > 0,
                });
            new ChoiceBox(cases.ToArray())
            {
                BackCase = new ChoiceBox.Case(() => _screenPlayer.AddNextScreen<CharacterView>())
            }.Show(); 
        }

        private Task UpgradeAsync(AttributeType x)
        {
            return _playerClient.UpgradeAsync(new Common.Models.UpgradeRequest()
            {
                Attribute = x,
            });
        }
    }
}
