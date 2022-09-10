using StalkerMUD.Client.Logic;
using StalkerMUD.Client.UI;
using StalkerMUD.Common;

namespace StalkerMUD.Client.Screens.Auth
{
    internal class LoginScreen : BaseAuthScreen
    {
        private readonly AuthClient _authClient;

        public LoginScreen(AuthClient authClient, ConnectionState connectionState, ScreenPlayer screenPlayer) :
            base(connectionState, screenPlayer)
        {
            _authClient = authClient;
        }

        public override string Name => "ВОЙТИ";

        protected override string RequestToken(string login, string? password)
        {
            var response = _authClient.LoginAsync(new Common.Models.AuthenticateRequest
            {
                Username = login,
                Password = password,

            }).Result;
            return response.Token;
        }
    }
}
