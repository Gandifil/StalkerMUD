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
            _moveQueue.Append(id);

            var response = new ActorResponse()
            {
                Id = id.GetHashCode(),
                Hp = actor.Hp,
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

        private void Attack(T currentActor)
        {
            throw new NotImplementedException(); // TODO
        }

        private void ShiftMoveQueue()
        {
            _moveQueue.Append(_moveQueue.First());
            _moveQueue.RemoveAt(0);
        }
    }
}
