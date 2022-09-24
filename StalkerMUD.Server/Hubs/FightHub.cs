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

        private static IRoom<string>? _room;

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

            foreach (var actorResponse in _room.Actors)
                await Clients.Caller.AddActorAsync(actorResponse);

            var actor = _room.Add(GetPlayerActorId(), PLAYER_COMMAND,
                await _paramatersCalculator.GetForAsync(await _users.GetAsync(GetUserId())));

            var allClients = Clients.All;
            await allClients.AddActorAsync(actor);
            await allClients.SendMessageAsync("Hello, World");

            if (isRoomNull)
            {
                await Clients.Caller.SelectActionAsync();
            }
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
            _room = new Room<string>();
            _room.OnMessage += OnMessage;
            _room.OnActorChanged += OnActorChanged;
            _room.Add(Guid.NewGuid().ToString(), MOB_COMMAND, 
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
            if (GetPlayerActorId() == _room?.CurrentActor)
                _room.Do(RoomAction.Attack);

            await SendSelectAction();
        }

        private async Task SendSelectAction()
        {
            var currentActor = _room.CurrentActor;
            if (currentActor.StartsWith(nameof(PLAYER_COMMAND)))
            {
                var connectionId = currentActor.Substring(nameof(PLAYER_COMMAND).Length);
                await Clients.Client(connectionId).SelectActionAsync();
            }
        }

        public async Task Skip()
        {
            if (GetPlayerActorId() == _room?.CurrentActor)
                _room.Do(RoomAction.Skip);

            await SendSelectAction();
        }
    }
}
