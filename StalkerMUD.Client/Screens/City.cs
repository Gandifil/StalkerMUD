using StalkerMUD.Client.UI;

namespace StalkerMUD.Client.Screens
{
    internal class City : Screen
    {
        public override string Name => "Город";

        public override string Description => "";

        private readonly ScreenPlayer _screenPlayer;

        public City( ScreenPlayer screenPlayer)
        {
            _screenPlayer = screenPlayer;
        }

        public override void Show()
        {
            base.Show();

            var choices = new ChoiceBox(new List<ChoiceBox.Case>{
                new ChoiceBox.Case("Арена")
                {
                    Action = () => _screenPlayer.AddNextScreen<City>(),
                },
                new ChoiceBox.Case("Персонаж")
                {
                    Action = () => _screenPlayer.AddNextScreen<CharacterView>(),
                },
                new ChoiceBox.Case("Торговец")
                {
                    Action = () => _screenPlayer.AddNextScreen<ShopScreen>(),
                },
            });

            choices.Show();
        }
    }
}
