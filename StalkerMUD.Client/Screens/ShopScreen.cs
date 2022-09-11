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

        public ChoiceBox GenerateChoices()
        {
            var items = _shopsClient.ShopsAsync().Result;
            var actions = new List<ChoiceBox.Case>();
            foreach (var shopPoint in items)
            {
                var cost = shopPoint.Cost;
                actions.Add(new ChoiceBox.Case()
                {
                    Name = $"{shopPoint.Name} - {cost} руб",
                    Action = () => { 
                        Buy(shopPoint.Id);
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

        private void Buy(int itemId)
        {
            _playerClient.BuyAsync(new BuyRequest() { ShopPointId = itemId }).Wait();
        }

        public override void Show()
        {
            UpdateMoney();

            base.Show();

            GenerateChoices().Show();
        }

        private void UpdateMoney()
        {
            _money = _playerClient.MoneyAsync().Result;
        }
    }
}
