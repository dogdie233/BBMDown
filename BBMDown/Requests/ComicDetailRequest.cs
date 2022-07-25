﻿using BBMDown.JsonConverters;

using System.Text.Json.Serialization;

namespace BBMDown.Requests
{
    public record ComicStyle(int Id, string Name);

    public class ComicDetailRequest : BaseRequest<ComicDetailRequest.Payload, ComicDetailResponseData>
    {
        public record Payload(int ComicId);

        public ComicDetailRequest(int comicId) : base(
            new Uri("https://manga.bilibili.com/twirp/comic.v1.Comic/ComicDetail?device=pc&platform=web"),
            HttpMethod.Post,
            new Payload(comicId))
        { }
    }

    public class EpModel
    {
        [JsonPropertyName("allow_wait_free")] public bool AllowWaitFree { get; set; } = false;
        [JsonPropertyName("chapter_id")] public int ChapterId { get; set; } = 0;
        /// <summary>
        /// 评论数
        /// </summary>
        [JsonPropertyName("comments")] public int CommentCount { get; set; } = 0;
        public string Cover { get; set; } = string.Empty;
        public int Extra { get; set; } = 0;
        public int Id { get; set; } = 0;
        [JsonPropertyName("image_count")] public int ImageCount { get; set; } = 0;
        [JsonPropertyName("is_in_free")] public bool IsInFree { get; set; } = false;
        [JsonPropertyName("is_locked")] public bool IsLocked { get; set; } = false;
        [JsonPropertyName("like_count")] public int LikeCount { get; set; } = 0;
        [JsonPropertyName("ord")] public int Order { get; set; } = 0;
        [JsonPropertyName("pay_gold")] public int PayGold { get; set; } = 0;
        [JsonPropertyName("pay_mode")] public int PayMode { get; set; } = 0;
        public string Progress { get; set; } = string.Empty;
        [JsonPropertyName("pub_time")][JsonConverter(typeof(DateTimeConverter_yyyyMMddHHmmss))] public DateTime? PubTime { get; set; }
        public int Read { get; set; } = 0;
        [JsonPropertyName("short_title")] public string ShortTitle { get; set; } = string.Empty;
        public ulong Size { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public int Type { get; set; } = 0;
        [JsonPropertyName("unlock_expire_at")][JsonConverter(typeof(DateTimeConverter_yyyyMMddHHmmss))] public DateTime? UnlockExpireAt { get; set; }
        [JsonPropertyName("unlock_type")] public int UnlockType { get; set; }
    }

    public class ComicDetailResponseData
    {
        public class FavComicInfoModel
        {
            [JsonPropertyName("has_fav_activity")] public bool HasFavActivity { get; set; } = false;
            [JsonPropertyName("fav_free_amount")] public int FavFreeAmount { get; set; }
            [JsonPropertyName("fav_coupon_type")] public int FavCouponType { get; set; }
        }

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("comic_type")] public int ComicType { get; set; } = 0;
        [JsonPropertyName("page_allow")] public int PageAllow { get; set; } = 0;
        [JsonPropertyName("horizontal_cover")] public string HorizontalCover { get; set; } = string.Empty;
        [JsonPropertyName("square_cover")] public string SquareCover { get; set; } = string.Empty;
        [JsonPropertyName("vertical_cover")] public string VerticalCover { get; set; } = string.Empty;
        [JsonPropertyName("author_name")] public string[] AuthorNames { get; set; } = Array.Empty<string>();
        public string[] Styles { get; set; } = Array.Empty<string>();
        [JsonPropertyName("last_ord")] public int LastOrder { get; set; } = 0;
        [JsonPropertyName("is_finish")][JsonConverter(typeof(BooleanNumberConverter))] public bool IsFinish { get; set; } = false;
        public int Status { get; set; } = 0;
        /// <summary>
        /// 是否为已追，0为未追，1为已追
        /// </summary>
        [JsonPropertyName("fav")][JsonConverter(typeof(BooleanNumberConverter))] public bool IsFav { get; set; } = false;
        /// <summary>
        /// 好像并不是读到哪了
        /// </summary>
        [JsonPropertyName("read_order")] public int ReadOrder { get; set; } = 0;
        /// <summary>
        /// 简介
        /// </summary>
        public string Evaluate { get; set; } = string.Empty;
        /// <summary>
        /// 总话数
        /// </summary>
        public int Total { get; set; } = 0;
        [JsonPropertyName("ep_list")] public EpModel[] EpList { get; set; } = Array.Empty<EpModel>();
        [JsonPropertyName("release_time")][JsonConverter(typeof(ReleaseTimeConverter))] public DateTime? ReleaseTime { get; set; }
        [JsonPropertyName("is_limit")][JsonConverter(typeof(BooleanNumberConverter))] public bool IsLimit { get; set; } = false;
        /// <summary>
        /// 读到哪了
        /// </summary>
        [JsonPropertyName("read_epid")] public int ReadEpid { get; set; } = 0;
        [JsonPropertyName("last_read_time")][JsonConverter(typeof(DateTimeConverter_yyyyMMddHHmmss))] public DateTime? LastReadTime { get; set; }
        [JsonPropertyName("is_download")][JsonConverter(typeof(BooleanNumberConverter))] public bool IsDownload { get; set; } = false;
        [JsonPropertyName("read_short_title")] public string ReadShortTitle { get; set; } = string.Empty;
        public ComicStyle[] Styles2 { get; set; } = Array.Empty<ComicStyle>();
        [JsonPropertyName("renewal_time")] public string RenewalTime { get; set; } = string.Empty;
        /// <summary>
        /// 最新一话短标题
        /// </summary>
        [JsonPropertyName("last_short_title")] public string LastShortTitle { get; set; } = string.Empty;
        [JsonPropertyName("discount_type")] public int DiscountType { get; set; } = 0;
        public double discount { get; set; } = 0;
        [JsonPropertyName("discount_end")][JsonConverter(typeof(DateTimeConverter_yyyyMMddHHmmss))] public DateTime? DiscountEnd { get; set; }
        [JsonPropertyName("no_reward")] public bool IsNoReward { get; set; } = false;
        [JsonPropertyName("batch_discount_type")] public int BatchDiscountType { get; set; } = 0;
        [JsonPropertyName("ep_discount_type")] public int EpDiscountType { get; set; } = 0;
        [JsonPropertyName("has_fav_activity")] public bool HasFavActivity { get; set; } = false;
        [JsonPropertyName("fav_free_amount")] public int FavFreeAmount { get; set; } = 0;
        /// <summary>
        /// 是不是等就免费了
        /// </summary>
        [JsonPropertyName("allow_wait_free")] public bool AllowWaitFree { get; set; } = false;
        /// <summary>
        /// 等待时间（小时）
        /// </summary>
        [JsonPropertyName("wait_hour")] public int WaitHour { get; set; } = 0;
        [JsonPropertyName("wait_free_at")][JsonConverter(typeof(DateTimeConverter_yyyyMMddHHmmss))] public DateTime? WaitFreeAt { get; set; }
        [JsonPropertyName("no_danmaku")][JsonConverter(typeof(BooleanNumberConverter))] public bool IsNoDanmaku { get; set; } = false;
        [JsonPropertyName("auto_pay_status")] public int AutoPayStatus { get; set; } = 0;
        [JsonPropertyName("no_month_ticket")] public bool IsNoMonthTicket { get; set; } = false;
        [JsonPropertyName("immersive")] public bool IsImmersive { get; set; } = false;
        [JsonPropertyName("no_discount")] public bool IsNoDiscount { get; set; } = false;
        [JsonPropertyName("show_type")] public int ShowType { get; set; } = 0;
        [JsonPropertyName("pay_mode")] public int PayMode { get; set; } = 0;
        public object[] Chapters { get; set; } = Array.Empty<object>();
        /// <summary>
        /// 又是简介
        /// </summary>
        [JsonPropertyName("classic_lines")] public string ClassicLines { get; set; } = string.Empty;
        [JsonPropertyName("pay_for_new")] public int PayForNew { get; set; } = 0;
        [JsonPropertyName("fav_comic_info")] public FavComicInfoModel FavComicInfo { get; set; } = new FavComicInfoModel();
        [JsonPropertyName("serial_status")] public int SerialStatus { get; set; } = 0;
        // series_info
        [JsonPropertyName("album_count")] public int AlbumCount { get; set; } = 0;
        [JsonPropertyName("wiki_id")] public int WikiId { get; set; } = 0;
        [JsonPropertyName("disable_coupon_amount")] public int DisableCouponAmount { get; set; } = 0;
        [JsonPropertyName("japan_comic")] public bool IsJapanComic { get; set; } = false;
        [JsonPropertyName("interact_value")] public int InteractValue { get; set; } = 0;
        [JsonPropertyName("temporary_finish_time")] public string TemporaryFinishTime { get; set; } = string.Empty;
        public object? Video { get; set; } = null;
        public string Introduction { get; set; } = string.Empty;
        [JsonPropertyName("comment_status")] public int CommentStatus { get; set; } = 0;
        [JsonPropertyName("no_screenshot")] public bool IsNoScreenshot { get; set; } = false;
        public int Type { get; set; } = 0;
        [JsonPropertyName("vomic_cvs")] public object[] VomicCVs { get; set; } = Array.Empty<object>();
        [JsonPropertyName("no_rank")] public bool IsNoRank { get; set; } = false;
        [JsonPropertyName("presale_eps")] public object[] PresaleEps { get; set; } = Array.Empty<object>();
        [JsonPropertyName("presale_text")] public string PresaleText { get; set; } = string.Empty;
        [JsonPropertyName("presale_discount")] public double PresaleDiscount { get; set; } = 0;
        [JsonPropertyName("no_leaderboard")] public bool IsNoLeaderboard { get; set; } = false;
    }
}
