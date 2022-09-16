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

        public string Token 
        {
            get => "";
            set
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    value ?? throw new ArgumentNullException());
            }
        }
    }
}
