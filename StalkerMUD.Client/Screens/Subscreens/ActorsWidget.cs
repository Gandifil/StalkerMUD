using StalkerMUD.Client.UI;
using StalkerMUD.Common.Models;

namespace StalkerMUD.Client.Screens.Subscreens
{
    internal class ActorsWidget: IScreen
    {
        private const int BLOCK_SIZE = 12;
        private readonly IEnumerable<ActorResponse> _actors;

        public ActorsWidget(IEnumerable<ActorResponse> actors)
        {
            _actors = actors;
        }

        public async Task Show()
        {
            var actors = _actors.OrderBy(x => x.Command).ToList();

            int? lastCommand = null;
            foreach (var actor in actors)
            {
                if ((lastCommand ?? actor.Command) != actor.Command)
                    Console.Write('|');
                Console.Write(TruncateString(actor.Name));
                Console.Write('|');
                lastCommand = actor.Command;
            }
            Console.WriteLine();

            lastCommand = null;
            foreach (var actor in actors)
            {
                if ((lastCommand ?? actor.Command) != actor.Command)
                    Console.Write('|');
                Console.Write(GetHpBar(actor.Hp, actor.MaxHp));
                Console.Write('|');
                lastCommand = actor.Command;
            }

        }

        private string GetHpBar(int hp, int maxHp)
        {
            var rate = hp * BLOCK_SIZE / maxHp;
            return new string('*', rate) + new string(' ', BLOCK_SIZE - rate);
        }

        private string TruncateString(string name)
        {
            if (name.Length <= BLOCK_SIZE)
                return name + new string(' ', BLOCK_SIZE - name.Length);
            else
                return name.Substring(0, BLOCK_SIZE - 3) + "...";
        }
    }
}
