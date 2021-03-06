using Microsoft.Extensions.Logging;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using UI.DataModels;

namespace UI.Services
{
    internal sealed class AuthenticationService : BaseHttpClientService
    {
        public AuthenticationService(HttpClient client, ILogger<AuthenticationService> logger) : base(client, logger)
        {
        }

        public async Task<string?> LoginAsync(Credentials credentials)
        {
            var json = JsonSerializer.Serialize(credentials);

            var request = new HttpRequestMessage(HttpMethod.Post, "auth/authenticate")
            {
                Content = new StringContent(json)
            };

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await Send(request);

            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

            return null;
        }

        public async Task<bool> RegisterAsync(Credentials credentials)
        {
            var json = JsonSerializer.Serialize(credentials);

            var request = new HttpRequestMessage(HttpMethod.Post, "auth/registeruser")
            {
                Content = new StringContent(json)
            };

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await Send(request);

            return response.IsSuccessStatusCode;
        }
    }
}