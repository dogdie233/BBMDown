using System.Net;
using System.Text.Json.Serialization;

namespace BBMDown.Models
{
    public class GeneralResponse<T>
    {
        [JsonIgnore] public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
