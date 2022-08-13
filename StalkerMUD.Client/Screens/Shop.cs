using StalkerMUD.Client.UI;
using StalkerMUD.Common;
using StalkerMUD.Common.Models;

namespace StalkerMUD.Client.Screens
{
    internal class Shop : Screen
    {
        public override string Name => "Торговец";

        public override string Description => $"Деньги: {_game.Player.Money}";

        private readonly Game _game;

        private readonly Screen _backScreen;

        public Shop(Game game, Screen backScreen)
        {
            _game = game;
            _backScreen = backScreen;
        }

        public override ChoiceBox GenerateChoices()
        {
            var shopPoints = new List<ShopPointResponse>();


            var actions = new List<ChoiceBox.Case>();
            foreach (var shopPoint in shopPoints)
            {
                var item = Item.Items[shopPoint.Id];
                var cost = shopPoint.Cost;
                actions.Add(new ChoiceBox.Case($"{item.Name} - {cost} руб")
                {
                    Action = () => { Buy(item, cost); },
                    IsEnabled = _game.Player.Money >= cost,
                });
            }
            return new ChoiceBox(actions)
            {
                BackScreen = _backScreen,
            };
        }

        private void Buy(Item item, int cost)
        {
            var player = _game.Player;

            if (player.Money >= cost)
            {
                player.Money -= cost;
                player.Items.Add(item);
            }
        }
    }
}
