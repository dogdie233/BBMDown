using BBMDown.Models;

namespace BBMDown.Requests
{
    public class ComicDetailRequest : BaseRequest<ComicDetailRequest.Payload, ComicDetailModel>
    {
        public record Payload(int ComicId);

        public ComicDetailRequest(int comicId) : base(
            new Uri("https://manga.bilibili.com/twirp/comic.v1.Comic/ComicDetail?device=pc&platform=web"),
            HttpMethod.Post,
            new Payload(comicId))
        { }
    }
}
