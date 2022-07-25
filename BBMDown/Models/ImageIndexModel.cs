using BBMDown.JsonConverters;

using System.Text.Json.Serialization;

namespace BBMDown.Models
{
    public class ImageInfoModel
    {
        public string Path { get; set; } = string.Empty;
        [JsonPropertyName("video_path")] public string VideoPath { get; set; } = string.Empty;
        [JsonPropertyName("video_size")] public string VideoSize { get; set; } = string.Empty;
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class ImageIndexModel
    {
        public string Path { get; set; } = string.Empty;
        public ImageInfoModel[] Images { get; set; } = Array.Empty<ImageInfoModel>();
        [JsonPropertyName("last_modified")][JsonConverter(typeof(DateTimeConverter_yyyyMMddHHmmss))] public DateTime? LastModified { get; set; }
        public string Host { get; set; } = string.Empty;
        public object? Video { get; set; }
    }
}
