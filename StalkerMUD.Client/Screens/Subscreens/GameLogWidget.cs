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
            _logs.TakeLast(_lineCount).ToList().ForEach(x => Console.WriteLine(x));
        }
    }
}
