using BBMDown.Requests;

using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace BBMDown
{
    public static class HttpHelper
    {
        private static readonly HttpClient _client = new HttpClient();
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
            
        };

        public static CookieContainer Cookies { get; private set; }

        static HttpHelper()
        {
            var handler = new HttpClientHandler();
            Cookies = handler.CookieContainer;
            _client = new HttpClient(handler);

            var name = Assembly.GetExecutingAssembly().GetName();
            var version = name.Version ?? new Version(1, 1, 1);
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36");
        }

        public static async Task<GeneralResponse<TResponse>> SendAsync<TResponse, TPayload>(BaseRequest<TPayload, TResponse> request)
        {
            using var payloadJsonStream = new MemoryStream();
            var message = new HttpRequestMessage(request.method, request.uri);
            if (request.payload != null)
            {
                JsonSerializer.Serialize(payloadJsonStream, request.payload, _jsonSerializerOptions);
                message.Content = new StringContent(Encoding.UTF8.GetString(payloadJsonStream.ToArray()), Encoding.UTF8, "application/json");
            }
            var response = await _client.SendAsync(message);
            var repJsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GeneralResponse<TResponse>>(repJsonString, _jsonSerializerOptions);
            if (result == null) throw new Exception($"Failed to deserialize a json string to {typeof(TResponse)}, json string: \"{repJsonString}\"");
            result.StatusCode = response.StatusCode;
            return result;
        }
        public static async Task DownloadFile(string url, Stream outStream)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _client.SendAsync(message);
            response.EnsureSuccessStatusCode();
            await response.Content.CopyToAsync(outStream);
        }

        public static void SetCookie(string key, string value)
        {
            Cookies.Add(new Cookie(key, value, "/", "manga.bilibili.com"));
        }
        public static void SetSessdata(string value) => SetCookie("SESSDATA", value);
    }
}
