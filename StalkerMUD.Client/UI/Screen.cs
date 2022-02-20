using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Client.UI
{
    internal abstract class Screen
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        //public ChoiceBox Choices { get; set; }

        public abstract ChoiceBox GenerateChoices();

        public Screen Show()
        {
            start:
            Console.Clear();

            if (!string.IsNullOrEmpty(Name))
                Console.WriteLine(Name.ToUpper());

            if (!string.IsNullOrEmpty(Description))
                Console.WriteLine(Description);

            Render();

            var choices = GenerateChoices();
            try
            {
                return choices.Show();
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("Неверный ввод! Попробуйте снова!");
#if DEBUG
                Console.WriteLine(e.Message);
#endif
                goto start;
            }
        }

        protected virtual void Render()
        {

        }

        public static void StartShowCycle(Screen startScreen)
        {
            var screen = startScreen;
            while (true)
                screen = screen.Show() ?? screen;
        }
    }
}
