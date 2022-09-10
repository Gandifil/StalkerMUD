using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Client.UI
{
    internal interface IScreen
    {
        void Show();
    }

    internal abstract class Screen : IScreen
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public virtual ChoiceBox GenerateChoices() { return null; }

        public virtual void Show()
        {
        //start:
            Console.Clear();

            if (!string.IsNullOrEmpty(Name))
                Console.WriteLine(Name.ToUpper());

            if (!string.IsNullOrEmpty(Description))
                Console.WriteLine(Description);

//            var choices = GenerateChoices();
//            try
//            {
//                return choices.Show();
//            }
//            catch (Exception e)
//            {
//                Console.Clear();
//                Console.WriteLine("Неверный ввод! Попробуйте снова!");
//#if DEBUG
//                Console.WriteLine(e.Message);
//#endif
//                goto start;
//            }
        }

        protected void Error(string message)
        {

        }
    }
}
