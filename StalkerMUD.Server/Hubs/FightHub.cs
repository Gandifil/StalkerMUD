using Microsoft.AspNetCore.SignalR;
using StalkerMUD.Common.Models;
using StalkerMUD.Server.Data;
using StalkerMUD.Server.Entities;
using StalkerMUD.Server.Services;

namespace StalkerMUD.Server.Hubs
{
    public class FightHub: Hub
    {
        private const int MOB_COMMAND = 0;
        private const int PLAYER_COMMAND = 1;

        private readonly IFightParamatersCalculator _paramatersCalculator;
        private readonly IRepository<Mob> _mobs;
        private readonly IRepository<User> _users;
        private readonly ILogger<FightHub> _logger;

        private static IRoom? _room;
        private static Dictionary<int, string>? roomIdConnectionId = new();

        public FightHub(IFightParamatersCalculator paramatersCalculator, IRepository<Mob> mobs, IRepository<User> users, ILogger<FightHub> logger)
        {
            _paramatersCalculator = paramatersCalculator;
            _mobs = mobs;
            _users = users;
            _logger = logger;
        }

        public async Task Start()
        {
            _logger.LogInformation("{ConnectionId} call Start", Context.ConnectionId);

            var isRoomNull = _room == null;
            if (isRoomNull)
                await InitializeRoom();
            _room.OnMessage += OnMessage;
            _room.OnActorChanged += OnActorChanged;

            foreach (var actorResponse in _room.Actors)
                await Clients.Caller.AddActorAsync(actorResponse);

            var actor = _room.Add(PLAYER_COMMAND,
                await _paramatersCalculator.GetForAsync(await _users.GetAsync(GetUserId())));

            roomIdConnectionId[actor.Id] = Context.ConnectionId;
            var allClients = Clients.All;
            await allClients.AddActorAsync(actor);
            await allClients.SendMessageAsync("Hello, World");

            if (isRoomNull)
            {
                await Clients.Caller.SelectActionAsync();
            }

            _room.OnMessage -= OnMessage;
            _room.OnActorChanged -= OnActorChanged;
        }
         
        private int GetUserId()
        {
            return Convert.ToInt32(Context.User?.Claims.First(x => x.Type == "id").Value);
        }

        private string GetPlayerActorId()
        {
            return nameof(PLAYER_COMMAND) + Context.ConnectionId;
        }

        private async Task InitializeRoom()
        {
            _room = new Room();
            _room.Add(MOB_COMMAND, 
                await _paramatersCalculator.GetForAsync(await _mobs.GetAsync(1)));
        }

        private void OnActorChanged(ActorChangeResponse actorChange)
        {
            Clients.All.ChangeActorAsync(actorChange);
        }

        private void OnMessage(string text)
        {
            Clients.All.SendMessageAsync(text);
        }

        public async Task Attack()
        {
            _room.OnMessage += OnMessage;
            _room.OnActorChanged += OnActorChanged;

            if ((int)Context.Items["id"] == _room?.CurrentActor)
                _room.Do(RoomAction.Attack);

            await SendSelectAction();

            _room.OnMessage -= OnMessage;
            _room.OnActorChanged -= OnActorChanged;
        }

        private async Task SendSelectAction()
        {
            if (roomIdConnectionId.TryGetValue(_room.CurrentActor, out var connectionId))
                await Clients.Client(connectionId).SelectActionAsync();
        }

        public async Task Skip()
        {
            if ((int)Context.Items["id"] == _room?.CurrentActor)
                _room.Do(RoomAction.Skip);

            await SendSelectAction();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            //if (stopCalled)
            //{
            //    Console.WriteLine(String.Format("Client {0} explicitly closed the connection.", Context.ConnectionId));
            //}
            //else
            //{
            //    Console.WriteLine(String.Format("Client {0} timed out .", Context.ConnectionId));
            //}
            _room = null;
            _logger.LogError(exception?.Message);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
