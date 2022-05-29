using BBMDown.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BBMDown
{
    public static class HttpHelper
    {
        private static readonly HttpClient _client = new HttpClient();
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
        };

        static HttpHelper()
        {
            _client = new HttpClient();
            var name = Assembly.GetExecutingAssembly().GetName();
            var version = name.Version ?? new Version(1, 1, 1);
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(name.Name ?? "BBMDown", $"{version.Major}.{version.Minor}.{version.Build}"));
        }

        public static async Task<GeneralResponse<T>> SendAsync<T, TPayload>(string url, HttpMethod method, TPayload payload)
        {
            using var payloadJsonStream = new MemoryStream();
            JsonSerializer.Serialize(payloadJsonStream, payload, jsonSerializerOptions);
            var message = new HttpRequestMessage(method, url);
            message.Content = new StringContent(Encoding.UTF8.GetString(payloadJsonStream.ToArray()), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(message);
            var repJsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GeneralResponse<T>>(repJsonString, jsonSerializerOptions);
            if (result == null) { throw new Exception($"Failed to deserialize a json string to {typeof(T)}, json string: \"{repJsonString}\""); }
            result.StatusCode = response.StatusCode;
            return result;
        }
    }
}
