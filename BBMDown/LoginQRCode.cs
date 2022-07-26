using QRCoder;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BBMDown
{
    public class LoginQRCode
    {
        private class LoginQRCodeRepModel
        {
            public JsonElement Data { get; set; }
        }

        public enum StatusType
        {
            NoConfirm = -5,
            NoScan = -4,
            TimedOut = -2,
            OauthKeyError = -1,
            Ok = 0,
        }

        private readonly HttpClient client;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
        };
        private readonly Timer timer;
        private TaskCompletionSource taskSource = new();
        private int retryCount = 0;

        public bool IsStarted { get; private set; }
        public StatusType Status { get; private set; } = StatusType.Ok;
        public string OauthKey { get; private set; } = string.Empty;
        public string QRUrl { get; private set; } = string.Empty;
        public string? SessdataResult { get; private set; } = null;
        private CookieContainer Cookies { get; set; }

        public LoginQRCode(Logger? logger = null)
        {
            var handler = new HttpClientHandler();
            Cookies = handler.CookieContainer;
            client = new HttpClient(handler);

            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36");
            timer = new Timer(OnTimerTick, null, Timeout.Infinite, Timeout.Infinite);
        }

        public async Task Start()
        {
            if (IsStarted) return;
            // request (qrcode)
            var response = await client.GetStringAsync("http://passport.bilibili.com/qrcode/getLoginUrl");
            var repJson = JsonSerializer.Deserialize<LoginQRCodeRepModel>(response, _jsonSerializerOptions);
            if (repJson == null) throw new JsonException();

            // read key
            var url = repJson.Data.GetProperty("url").GetString();
            var key = repJson.Data.GetProperty("oauthKey").GetString();
            if (url == null || key == null) throw new JsonException();
            
            QRUrl = url;
            OauthKey = key;
            
            timer.Change(1000, 1000);
            Status = StatusType.NoScan;
            IsStarted = true;
        }

        /// <summary>
        /// Get a Task, it will complete when login succeed or failed.
        /// </summary>
        /// <returns>exception is not null when error</returns>
        public Task GetLoginTask()
        {
            return taskSource.Task;
        }

        private async void OnTimerTick(object? state)
        {
            // request
            var content = new FormUrlEncodedContent(new KeyValuePair<string, string>[] { new("oauthKey", OauthKey) });
            LoginQRCodeRepModel? repJson;
            try
            {
                var response = await client.PostAsync("http://passport.bilibili.com/qrcode/getLoginInfo", content);
                response.EnsureSuccessStatusCode();
                var str = await response.Content.ReadAsStringAsync();

                // deserialize
                repJson = JsonSerializer.Deserialize<LoginQRCodeRepModel>(str, _jsonSerializerOptions);
                if (repJson == null) throw new Exception("Json 反序列化结果为空, json: " + str);
            }
            catch (Exception e)
            {
                retryCount++;
                if (retryCount < 5) return;

                // login failed
                taskSource.SetException(e);
                timer.Dispose();
                return;
            }

            // check data
            switch (repJson.Data.ValueKind)
            {
                case (JsonValueKind.Number): // when get error
                    {
                        Status = (StatusType)repJson.Data.GetInt32();
                        if (Status == StatusType.TimedOut || Status == StatusType.OauthKeyError)
                        {
                            taskSource.SetException(new Exception("Failed to login: timeout or oautKeyError"));
                            break; // dispose the timer
                        }
                        return;
                    }
                case (JsonValueKind.Object): // success
                    {
                        var cookies = Cookies.GetAllCookies();
                        var sessCookie = cookies["SESSDATA"];
                        if (sessCookie != null)
                        {
                            SessdataResult = sessCookie.Value;
                            break; // dispose the timer
                        }
                        taskSource.SetException(new Exception("阿b怎么没有设置sessdata捏"));
                        break;
                    }
                default: return;
            }

            timer.Dispose();
            taskSource.SetResult();
        }
    }
}
