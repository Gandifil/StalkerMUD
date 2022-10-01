using StalkerMUD.Client.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Client.Screens.Subscreens
{
    internal class GameLogWidget: IScreen
    {
        private readonly IEnumerable<string> _logs;
        private readonly int _lineCount;

        public GameLogWidget(IEnumerable<string> logs, int lineCount)
        {
            _logs = logs;
            _lineCount = lineCount;
        }

        public async Task Show()
        {
            _logs.Select((x, i) => (x, i)).TakeLast(_lineCount)
                .ToList().ForEach(x => ColoredWriteLine(x.i, x.x));
        }

        private static readonly ConsoleColor COLOR = ConsoleColor.Yellow;

        private void ColoredWriteLine(int index, string str)
        {
            str = $"{index}: {str}";
            var current = Console.ForegroundColor;

            foreach (var substr in str.Split(new char[] { '[', ']' }))
            {
                Console.Write(substr);
                Console.ForegroundColor = Console.ForegroundColor == COLOR ? current : COLOR;
            }

            Console.ForegroundColor = current;
            Console.WriteLine();
        }
    }
}
