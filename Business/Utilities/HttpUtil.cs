using System.Text;
using System.Text.Json;

namespace Business.Utilities
{
    public class HttpUtil
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Thực hiện yêu cầu GET
        /// </summary>
        public static async Task<string> GetAsync(string url, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            // Thêm Authorization header nếu cần
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Thực hiện yêu cầu POST
        /// </summary>
        public static async Task<string> PostAsync(string url, object body, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Thực hiện yêu cầu PUT
        /// </summary>
        public static async Task<string> PutAsync(string url, object body, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Thực hiện yêu cầu DELETE
        /// </summary>
        public static async Task<bool> DeleteAsync(string url, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
