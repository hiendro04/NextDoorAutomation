using RestSharp;
using System.Text.Json;

namespace Business.Helpers
{
    public class GoLoginApiHelper
    {
        private readonly string _apiToken;
        private readonly string _baseApiUrl = "https://api.gologin.com";

        public GoLoginApiHelper(string apiToken)
        {
            _apiToken = apiToken;
        }

        public async Task<string> GetBrowserProfile(string profileId)
        {
            var client = new RestClient($"https://api.gologin.com/browser/{profileId}");
            var request = new RestRequest();
            request.Method = Method.Get;

            request.AddHeader("Authorization", $"Bearer {_apiToken}");

            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Content; // Thông tin cấu hình browser
            }
            else
            {
                throw new Exception("Không thể lấy thông tin profile");
            }
        }

        public async Task<string> StartProfileAsync(string profileId)
        {
            var client = new RestClient(_baseApiUrl);
            var request = new RestRequest($"/browser/{profileId}/start", Method.Post);
            request.AddHeader("Authorization", $"Bearer {_apiToken}");

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to start profile: {response.Content}");
            }

            var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(response.Content);
            return jsonResponse?["webSocketDebuggerUrl"]?.ToString() ?? throw new Exception("webSocketDebuggerUrl not found.");
        }

        public async Task StopProfileAsync(string profileId)
        {
            var client = new RestClient(_baseApiUrl);
            var request = new RestRequest($"/browser/{profileId}/stop", Method.Post);
            request.AddHeader("Authorization", $"Bearer {_apiToken}");
            await client.ExecuteAsync(request);
        }
    }
}