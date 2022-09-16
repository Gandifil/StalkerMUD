using Microsoft.AspNetCore.SignalR.Client;
using StalkerMUD.Client.UI;
using StalkerMUD.Common.Models;

namespace StalkerMUD.Client.Screens
{
    internal class FightScreen : Screen
    {
        public override string Name => "БОЙ";

        private readonly HubConnection _connection;

        public FightScreen(HubConnection connection)
        {
            _connection = connection;
        }

        public override async Task Show()
        {
            await base.Show();

            _connection.On<ActorResponse>("newActor", actor => Console.WriteLine(actor.Name));

            _connection.On("selectAction", () =>
            {
                var choices = new ChoiceBox(new List<ChoiceBox.Case>
                {
                    new ChoiceBox.Case(() => _connection.InvokeAsync("Attack"), "Стрелять"),
                    new ChoiceBox.Case(() => _connection.InvokeAsync("Skip"), "Пропустить"),
                });

                choices.Show();
            });

            await _connection.StartAsync();
            await _connection.InvokeAsync("Start");
            Console.ReadLine();
        }
    }
}
