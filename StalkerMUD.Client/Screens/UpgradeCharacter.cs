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

        public override string Description => "";

        private readonly IPlayerClient _playerClient;
        private readonly ScreenPlayer _screenPlayer;

        public UpgradeCharacter(IPlayerClient playerClient, ScreenPlayer screenPlayer)
        {
            _playerClient = playerClient;
            _screenPlayer = screenPlayer;
        }

        public override void Show()
        {
            base.Show();

            var player = _playerClient.PlayerAsync().Result;

            Console.WriteLine($"Свободных очков: {player.AttributeFreePoints}");


            var cases = ((AttributeType[])Enum.GetValues(typeof(AttributeType)))
                .Select(x => new ChoiceBox.Case()
                {
                    Name = $"{Attributes.Names[x]} - {player.Attributes[x]}",
                    Action = () => { Upgrade(x); _screenPlayer.Restart(); },
                    IsEnabled = player.AttributeFreePoints > 0,
                });
            new ChoiceBox(cases.ToArray())
            {
                BackCase = new ChoiceBox.Case(() => _screenPlayer.AddNextScreen<CharacterView>())
            }.Show(); 
        }

        private void Upgrade(AttributeType x)
        {
            _playerClient.UpgradeAsync(new Common.Models.UpgradeRequest()
            {
                Attribute = x,
            }).Wait();
        }
    }
}
