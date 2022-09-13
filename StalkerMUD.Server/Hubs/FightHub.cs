﻿using Microsoft.AspNetCore.SignalR;
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

        private IRoom<string>? _room;

        public FightHub(IFightParamatersCalculator paramatersCalculator, IRepository<Mob> mobs, IRepository<User> users)
        {
            _paramatersCalculator = paramatersCalculator;
            _mobs = mobs;
            _users = users;
        }

        public async Task Start()
        {
            if (_room == null)
                await InitializeRoom();

            foreach (var actorResponse in _room.Actors)
                await Clients.Caller.SendCoreAsync("newActor", new object[] { actorResponse });

            var actor = _room.Add(GetPlayerActorId(), PLAYER_COMMAND,
                await _paramatersCalculator.GetForAsync(await _users.GetAsync(GetUserId())));

            await Clients.All.SendCoreAsync("newActor", new object[] { actor });
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
            _room.Add(Guid.NewGuid().ToString(), MOB_COMMAND, 
                await _paramatersCalculator.GetForAsync(await _mobs.GetAsync(1)));
        }

        public void Attack()
        {
            if (GetPlayerActorId() == _room?.CurrentActor)
                _room.Do(RoomAction.Attack);
        }

        public void Skip()
        {
            if (GetPlayerActorId() == _room?.CurrentActor)
                _room.Do(RoomAction.Skip);
        }
    }
}