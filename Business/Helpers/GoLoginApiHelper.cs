using Business.Utilities;
using PuppeteerSharp;
using System.Text.Json;

namespace Business.Helpers
{
    public class GoLoginApiHelper
    {
        private readonly string _apiToken;
        private readonly string _baseApiUrl = "https://api.gologin.com";
        private readonly string _baseLocalApiUrl = "http://localhost:36912";

        public GoLoginApiHelper(string apiToken)
        {
            _apiToken = apiToken;
        }

        public async Task<string> GetBrowserProfile(string profileId)
        {
            var url = $"https://api.gologin.com/browser/{profileId}";
            var response = await HttpUtil.GetAsync(url, _apiToken);
            return response;
        }

        public async Task<string> StartProfileAsync(string profileId)
        {
            var url = _baseLocalApiUrl + "/browser/start-profile";
            var body = new
            {
                profileId = profileId,
                sync = true,
            };

            var response = await HttpUtil.PostAsync(url, body, _apiToken);
            return JsonSerializer.Deserialize<Dictionary<string, object>>(response)?["wsUrl"]?.ToString() ?? throw new Exception("webSocketDebuggerUrl not found.");
        }

        public async Task StopProfileAsync(string profileId)
        {
            var url = _baseLocalApiUrl + "/browser/stop-profile";
            var body = new
            {
                profileId = profileId,
            };

            await HttpUtil.PostAsync(url, body, _apiToken);
        }
    }
}