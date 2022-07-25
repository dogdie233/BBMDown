using BBMDown.JsonConverters;

using System.Text.Json.Serialization;

namespace BBMDown.Models
{
    public class ImageTokenModel
    {
        public string Url { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
