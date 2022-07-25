using BBMDown.JsonConverters;

using System.Text.Json.Serialization;

namespace BBMDown.Models
{
    public class EpModel
    {
        [JsonPropertyName("allow_wait_free")] public bool AllowWaitFree { get; set; } = false;
        [JsonPropertyName("chapter_id")] public int ChapterId { get; set; }
        /// <summary>
        /// 评论数
        /// </summary>
        [JsonPropertyName("comments")] public int CommentCount { get; set; }
        public string Cover { get; set; } = string.Empty;
        public int Extra { get; set; }
        public int Id { get; set; }
        [JsonPropertyName("image_count")] public int ImageCount { get; set; }
        [JsonPropertyName("is_in_free")] public bool IsInFree { get; set; } = false;
        [JsonPropertyName("is_locked")] public bool IsLocked { get; set; } = false;
        [JsonPropertyName("like_count")] public int LikeCount { get; set; }
        [JsonPropertyName("ord")] public int Order { get; set; }
        [JsonPropertyName("pay_gold")] public int PayGold { get; set; }
        [JsonPropertyName("pay_mode")] public int PayMode { get; set; }
        public string Progress { get; set; } = string.Empty;
        [JsonPropertyName("pub_time")][JsonConverter(typeof(DateTimeConverter_yyyyMMddHHmmss))] public DateTime? PubTime { get; set; }
        public int Read { get; set; }
        [JsonPropertyName("short_title")] public string ShortTitle { get; set; } = string.Empty;
        public ulong Size { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Type { get; set; }
        [JsonPropertyName("unlock_expire_at")][JsonConverter(typeof(DateTimeConverter_yyyyMMddHHmmss))] public DateTime? UnlockExpireAt { get; set; }
        [JsonPropertyName("unlock_type")] public int UnlockType { get; set; }
    }
}
