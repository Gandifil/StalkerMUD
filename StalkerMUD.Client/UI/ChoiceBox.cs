using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Client.UI
{
    internal class ChoiceBox
    {
        private const char QUIT_KEY = 'q';

        public class Case
        {
            public string Name { get; }

            public Action? Action { get; set; }

            public bool IsEnabled { get; set; } = true;

            public Screen? Screen { get; set; }

            public ConsoleColor Color { get; set; } = ConsoleColor.Gray;

            public Case(string name)
            {
                Name = name;
            }
        }

        private readonly List<Case> _cases;

        public Screen? BackScreen { get; set; }

        public Case? EnterCase { get; set; }

        public ChoiceBox([NotNull] ICollection<Case> cases)
        {
            _cases = cases.ToList();
        }

        public ChoiceBox(params Case[] cases)
        {
            _cases = cases.ToList();
        }

        public Screen Show()
        {
            if (BackScreen is not null)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{QUIT_KEY}. Назад");
            }

            if (EnterCase is not null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Enter. {EnterCase.Name}");
            }

            for (int i = 0; i < _cases.Count; i++)
            {
                Console.ForegroundColor = _cases[i].IsEnabled ? _cases[i].Color : ConsoleColor.Red;
                Console.WriteLine($"{i + 1}. {_cases[i].Name}");
            }
            Console.ForegroundColor = ConsoleColor.Gray;

            var key = Console.ReadKey();
            if (BackScreen is not null && (key.KeyChar == QUIT_KEY || key.KeyChar == 'й'))
                return BackScreen;

            Case scase = GetSelectedCase(key);
            scase.Action?.Invoke();
            return scase.Screen;
        }

        private Case GetSelectedCase(ConsoleKeyInfo key)
        {
            if (EnterCase is not null && key.Key == ConsoleKey.Enter)
                return EnterCase;

            return _cases[int.Parse(key.KeyChar.ToString()) - 1];
        }
    }
}
