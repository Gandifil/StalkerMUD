using StalkerMUD.Common.Models;

namespace StalkerMUD.Server.Services
{
    public enum RoomAction
    {
        Skip,
        Attack,
    }

    public interface IRoom
    {
        ActorResponse Add(int command, bool aiEnabled, FightParametersResponse parameters);

        void Do(RoomAction action);

        int CurrentActor { get; }

        int? Winner { get; }

        IReadOnlyCollection<ActorResponse> Actors { get; }

        delegate void Message(string text);
        event Message? OnMessage;

        delegate void ActorChanged(ActorChangeResponse actorChange);
        event ActorChanged? OnActorChanged;
    }

    public class Room : IRoom
    {
        private class Actor
        {
            public int Id { get; set; }

            public int Hp { get; set; }

            public int Command { get; set; }

            public bool AiEnabled { get; set; }

            public FightParametersResponse Parameters { get; set; }
        }

        private readonly Dictionary<int, Actor> _actors = new();
        private readonly List<int> _moveQueue = new();

        public int CurrentActor => _moveQueue.First();

        public int? Winner { get; private set; }

        private List<ActorResponse> _actorResponses = new();

        public event IRoom.Message? OnMessage;
        public event IRoom.ActorChanged? OnActorChanged;

        public IReadOnlyCollection<ActorResponse> Actors => _actorResponses;

        private int _idCounter = 0;

        public ActorResponse Add(int command, bool aiEnabled, FightParametersResponse parameters)
        {
            var actor = new Actor()
            {
                Id = _idCounter++,
                Hp = parameters.MaxHP,
                Command = command,
                AiEnabled = aiEnabled,
                Parameters = parameters,
            };
            if (aiEnabled)
                _moveQueue.Add(actor.Id);
            else
                _moveQueue.Insert(_actors.Values.All(x => x.AiEnabled) ? 0 : 1, actor.Id);
            _actors.Add(actor.Id, actor);

            var response = new ActorResponse()
            {
                Id = actor.Id,
                Hp = actor.Hp,
                MaxHp = parameters.MaxHP,
                Command = command,
                Name = parameters.Name,
            };
            _actorResponses.Add(response);
            return response;
        }

        public void Do(RoomAction action)
        {
            switch (action)
            {
                case RoomAction.Skip:
                    break;
                case RoomAction.Attack:
                    Attack(CurrentActor);
                    break;
                default:
                    break;
            }

            ShiftMoveQueue();
        }

        private void Attack(int currentActorId)
        {
            var actor = _actors[currentActorId];
            var target = _actors.Values.First(x => x.Command != actor.Command);

            // strike logic
            var accuracy = actor.Parameters.Attributes[Common.AttributeType.Accuracy];
            var dodge = target.Parameters.Attributes[Common.AttributeType.Dodge];
            var seed = Random.Shared.Next(accuracy + dodge);
            seed -= dodge;
            if (seed < 0)
            {
                OnMessage?.Invoke($"[{actor.Parameters.Name}] не смог попасть по [{target.Parameters.Name}]");
                return;
            }

            var isCriticaly = Random.Shared.Next(100) < actor.Parameters.CritPercent;

            var damage = isCriticaly ? actor.Parameters.CritDamage : actor.Parameters.Damage;
            var clearDamage = Math.Max(damage - target.Parameters.Resistance, 0);

            target.Hp = Math.Max(target.Hp - clearDamage, 0);
            OnActorChanged?.Invoke(
                new ActorChangeResponse
                {
                    Id = target.Id.GetHashCode(),
                    Type = ActorChangeResponse.ParameterType.Hp,
                    Value = target.Hp
                });
            OnMessage?.Invoke($"[{actor.Parameters.Name}] нанес [{clearDamage}] ед. урона по [{target.Parameters.Name}]");
        }

        private void ShiftMoveQueue()
        {
            while(true)
            {
                TurnQueue();
                if (_actors[CurrentActor].AiEnabled)
                    Attack(CurrentActor);
                else
                    return;
            }
        }

        private void TurnQueue()
        {
            _moveQueue.Add(_moveQueue.First());
            _moveQueue.RemoveAt(0);
        }
    }
}
