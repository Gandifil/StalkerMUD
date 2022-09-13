﻿using StalkerMUD.Client.UI;

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

            new ChoiceBox(new List<ChoiceBox.Case>{
                new ChoiceBox.Case(() => _screenPlayer.AddNextScreen<FightScreen>(), "Арена"),
                new ChoiceBox.Case(() => _screenPlayer.AddNextScreen<CharacterView>(), "Персонаж"),
                new ChoiceBox.Case(() => _screenPlayer.AddNextScreen<ShopScreen>(), "Торговец"),
            }).Show();
        }
    }
}
