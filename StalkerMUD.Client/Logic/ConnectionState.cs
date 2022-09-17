using System.Net.Http.Headers;

namespace StalkerMUD.Client.Logic
{
    internal class ConnectionState
    {
        private readonly HttpClient _httpClient;

        public ConnectionState(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private string _token;

        public string Token 
        {
            get => _token;
            set
            {
                _token = value;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    value ?? throw new ArgumentNullException());
            }
        }
    }
}
