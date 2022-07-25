using BBMDown.Models;

using System.Text;
using System.Text.Json;

namespace BBMDown.Requests
{
    public class ImageTokenRequest : BaseRequest<ImageTokenRequest.Payload, ImageTokenModel[]>
    {
        public class Payload
        {
            public string Urls { get; set; } = string.Empty;
            
            public Payload() { }
            public Payload(string[] urls)
            {
                using var ms = new MemoryStream();
                JsonSerializer.Serialize(ms, urls);
                using var reader = new StreamReader(ms, Encoding.UTF8);
                ms.Seek(0, SeekOrigin.Begin);
                Urls = reader.ReadToEnd();
            }
        }

        public ImageTokenRequest(string[] urls) : base(
            new Uri("https://manga.bilibili.com/twirp/comic.v1.Comic/ImageToken?device=pc&platform=web"),
            HttpMethod.Post,
            new Payload(urls))
        { }
    }
}
