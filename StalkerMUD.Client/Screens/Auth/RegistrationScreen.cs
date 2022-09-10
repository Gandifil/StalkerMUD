using StalkerMUD.Client.Logic;
using StalkerMUD.Client.UI;
using StalkerMUD.Common;

namespace StalkerMUD.Client.Screens.Auth
{
    internal class RegistrationScreen : BaseAuthScreen
    {
        private readonly AuthClient _authClient;

        public RegistrationScreen(AuthClient authClient, ConnectionState connectionState, ScreenPlayer screenPlayer):
            base(connectionState, screenPlayer)
        {
            _authClient = authClient;
        }

        public override string Name => "Регистрация";

        protected override string RequestToken(string login, string? password)
        {
            var response = _authClient.RegisterAsync(new Common.Models.AuthenticateRequest
            {
                Username = login,
                Password = password,

            }).Result;
            return response.Token;
        }
    }
}
