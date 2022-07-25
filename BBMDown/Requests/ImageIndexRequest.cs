using BBMDown.Models;

using System.Text.Json.Serialization;

namespace BBMDown.Requests
{
    public class ImageIndexRequest : BaseRequest<ImageIndexRequest.Payload, ImageIndexModel>
    {
        public class Payload
        {
            [JsonPropertyName("ep_id")] public int EpId { get; set; }

            public Payload() { }
            public Payload(int epId) { EpId = epId; }
        }

        public ImageIndexRequest(int epId) : base(
            new Uri("https://manga.bilibili.com/twirp/comic.v1.Comic/GetImageIndex?device=pc&platform=web"),
            HttpMethod.Post,
            new Payload(epId))
        { }
    }
}
