using StalkerMUD.Client.UI;
using StalkerMUD.Common;
using StalkerMUD.Common.Models;

namespace StalkerMUD.Client.Screens
{
    internal class ShopScreen : Screen
    {
        public override string Name => "Магазин";

        public override string Description => $"Деньги: {_money}";

        private readonly IPlayerClient _playerClient;
        private readonly ShopsClient _shopsClient;
        private readonly ScreenPlayer _screenPlayer;
        private int _money;

        public ShopScreen(IPlayerClient client, ShopsClient shopsClient, ScreenPlayer screenPlayer)
        {
            _playerClient = client;
            _shopsClient = shopsClient;
            _screenPlayer = screenPlayer;
        }

        public async Task<ChoiceBox> GenerateChoices()
        {
            var items = await _shopsClient.ShopsAsync();
            var actions = new List<ChoiceBox.Case>();
            foreach (var shopPoint in items)
            {
                var cost = shopPoint.Cost;
                actions.Add(new ChoiceBox.Case()
                {
                    Name = $"{shopPoint.Name} - {cost} руб",
                    Action = () => { 
                        Buy(shopPoint.Id).Wait();
                        _screenPlayer.Restart();
                    },
                    IsEnabled = _money >= cost,
                });
            }
            return new ChoiceBox(actions)
            {
                BackCase = new ChoiceBox.Case(() =>  _screenPlayer.AddNextScreen<City>()),
            };
        }

        private Task Buy(int itemId)
        {
            return _playerClient.BuyAsync(new BuyRequest() { ShopPointId = itemId });
        }

        public override async Task Show()
        {
            await UpdateMoneyAsync();

            await base.Show();

            var choices = await GenerateChoices();
            choices.Show();
        }

        private async Task UpdateMoneyAsync()
        {
            _money = await _playerClient.MoneyAsync();
        }
    }
}
