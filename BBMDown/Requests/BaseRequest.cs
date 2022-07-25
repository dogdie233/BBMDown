using System.Net;
using System.Text.Json.Serialization;

namespace BBMDown.Requests
{
    public abstract class BaseRequest<TPayload, TResponse>
    {
        public readonly HttpMethod method;
        public readonly Uri uri;
        public readonly TPayload? payload;

        protected BaseRequest(Uri uri, HttpMethod method, TPayload? payload)
        {
            this.uri = uri;
            this.method = method;
            this.payload = payload;
        }
    }

    public class GeneralResponse<T>
    {
        [JsonIgnore] public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        // 这玩意既可能是int又可能是string，程序员ybb
        // public int Code { get; set; } = 0;
        public string Msg { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
