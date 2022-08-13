using StalkerMUD.Client.UI;
using StalkerMUD.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Client.Screens
{
    internal class RegistrationScreen : Screen
    {
        public override string Name => "Регистрация";

        public override string Description => string.Empty;

        public override ChoiceBox GenerateChoices()
        {
            Console.Write("Логин: ");
            var login = Console.ReadLine();

            Console.Write("Пароль: ");
            var password = Console.ReadLine();

            var client = new AuthClient("https://localhost:443", new HttpClient());
            client.JsonSerializerSettings.PropertyNameCaseInsensitive = true;

            var response = client.RegisterAsync(new Common.Models.AuthenticateRequest
            {
                Username = login,
                Password = password,

            }).Result;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", 
                response?.Token ?? throw new ArgumentNullException());
            var shops = new ShopsClient("https://localhost:443", httpClient);
            shops.JsonSerializerSettings.PropertyNameCaseInsensitive = true;

            var response2 = shops.ShopsAsync().Result;

            return new ChoiceBox(new List<ChoiceBox.Case>{
                new ChoiceBox.Case("Зарегистрироваться")
                {
                    Screen = new RegistrationScreen(),
                },
            });
        }
    }
}
