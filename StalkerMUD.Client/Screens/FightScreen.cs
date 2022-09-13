using Microsoft.AspNetCore.SignalR.Client;
using StalkerMUD.Client.UI;
using StalkerMUD.Common.Models;

namespace StalkerMUD.Client.Screens
{
    internal class FightScreen : Screen
    {
        public override string Name => "БОЙ";

        public override string Description => null;

        private readonly HubConnection _connection;

        public FightScreen(HubConnection connection)
        {
            _connection = connection;
        }

        public override void Show()
        {
            base.Show();

            _connection.On<ActorResponse>("newActor", actor => Console.WriteLine(actor.Name));

            _connection.StartAsync().Wait();
            _connection.InvokeAsync("Start").Wait();
            Console.ReadLine();
        }
    }
}
