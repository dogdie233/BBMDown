using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace BBMDown.JsonConverters
{
    /// <summary>
    /// yyyy-MM-dd HH:mm:ss
    /// </summary>
    public sealed class DateTimeConverter_yyyyMMddHHmmss : JsonConverter<DateTime?>
    {
        static readonly CultureInfo zhCN = new CultureInfo("zh-CN");

        // 可能在别的国家会有bug
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString() ?? "";
            if (str.Trim().Length == 0 || str == "0000-00-00 00:00:00") return null;
            try
            {
                var date = DateTime.ParseExact(str, "yyyy-MM-dd HH:mm:ss", zhCN, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal);
                return date;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.HasValue ? value.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
        }
    }

    /// <summary>
    /// yyyy.MM.dd
    /// </summary>
    public sealed class ReleaseTimeConverter : JsonConverter<DateTime?>
    {
        static readonly CultureInfo zhCN = new CultureInfo("zh-CN");

        // 可能在别的国家会有bug
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString() ?? "";
            if (str.Trim().Length == 0 || str == "0000-00-00") return null;
            try
            {
                var date = DateTime.ParseExact(str, "yyyy-MM-dd", zhCN, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal);
                return date;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.HasValue ? value.Value.ToString("yyyy-MM-dd") : "");
        }
    }
}
