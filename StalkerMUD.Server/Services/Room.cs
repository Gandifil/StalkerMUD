using StalkerMUD.Common.Models;

namespace StalkerMUD.Server.Services
{
    public enum RoomAction
    {
        Skip,
        Attack,
    }

    public interface IRoom<T>
    {
        ActorResponse Add(T id, int command, FightParametersResponse parameters);

        void Do(RoomAction action);

        T CurrentActor { get; }

        T? Winner { get; }

        IReadOnlyCollection<ActorResponse> Actors { get; }


        delegate void Message(string text);
        event Message? OnMessage;

        delegate void ActorChanged(ActorChangeResponse actorChange);
        event ActorChanged? OnActorChanged;
    }

    public class Room<T> : IRoom<T>
    {
        private class Actor
        {
            public T Id { get; set; }

            public int Hp { get; set; }

            public int Command { get; set; }

            public FightParametersResponse Parameters { get; set; }
        }

        private readonly Dictionary<T, Actor> _actors = new();
        private readonly List<T> _moveQueue = new();

        public T CurrentActor => _moveQueue.First();

        public T? Winner { get; private set; }

        private List<ActorResponse> _actorResponses = new();

        public event IRoom<T>.Message? OnMessage;
        public event IRoom<T>.ActorChanged? OnActorChanged;

        public IReadOnlyCollection<ActorResponse> Actors => _actorResponses;

        public ActorResponse Add(T id, int command, FightParametersResponse parameters)
        {
            var actor = new Actor()
            {
                Id = id,
                Hp = parameters.MaxHP,
                Command = command,
                Parameters = parameters,
            };
            _actors.Add(id, actor);
            _moveQueue.Add(id);

            var response = new ActorResponse()
            {
                Id = id.GetHashCode(),
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

        private void Attack(T currentActorId)
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
            _moveQueue.Add(_moveQueue.First());
            _moveQueue.RemoveAt(0);
        }
    }
}
