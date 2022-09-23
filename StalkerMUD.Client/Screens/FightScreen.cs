using Microsoft.AspNetCore.SignalR.Client;
using StalkerMUD.Client.Screens.Subscreens;
using StalkerMUD.Client.UI;
using StalkerMUD.Common.Models;

namespace StalkerMUD.Client.Screens
{
    internal class FightScreen : Screen
    {
        public override string Name => "БОЙ";

        private readonly HubConnection _connection;
        private readonly CancellationToken _cancellationToken = new CancellationToken();
        private List<ActorResponse> _actors = new();
        private List<ChoiceBox.Case> _choices = new();
        private List<string> _messages = new();

        public FightScreen(HubConnection connection)
        {
            _connection = connection;
        }

        public override async Task Show()
        {
            var actors = new List<ActorResponse>();

            _connection.On<ActorResponse>("newActor", OnAddActor);
            _connection.On("selectAction", OnSelectAction);
            _connection.On<string>("message", OnMessage);

            await _connection.StartAsync();
            await _connection.InvokeAsync("Start");

            while (!_cancellationToken.IsCancellationRequested)
                await Task.Delay(100);
        }

        private void OnMessage(string message)
        {
            _messages.Add(message);
            Rerender();
        }

        private void OnSelectAction()
        {
            _choices = new List<ChoiceBox.Case>()
                {
                    new ChoiceBox.Case(() => _connection.InvokeAsync("Attack"), "Стрелять"),
                    new ChoiceBox.Case(() => _connection.InvokeAsync("Skip"), "Пропустить"),
                };

            Rerender();
        }

        private void OnAddActor(ActorResponse actor)
        {
            _actors.Add(actor);
            Rerender();
        }

        private async Task Rerender()
        {
            Console.Clear();
            await new ActorsWidget(_actors).Show();
            await new GameLogWidget(_messages, 10).Show();
            if (_choices.Any())
                new ChoiceBox(_choices).Show();
        }
    }
}
