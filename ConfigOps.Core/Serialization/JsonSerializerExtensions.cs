using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConfigOps.Core.Serialization
{
    internal static class JsonSerializerExtensions
    {
        private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            IgnoreReadOnlyProperties = true,
            IgnoreReadOnlyFields = true,
            IncludeFields = true,
            MaxDepth = 100,
            WriteIndented = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            PropertyNameCaseInsensitive = true
        };

        public static byte[] SerializeToJsonBytes(this object value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value, value.GetType(), jsonSerializerOptions);
        }

        public static string SerializeToJson(this object value)
        {
            return JsonSerializer.Serialize(value, value.GetType(), jsonSerializerOptions);
        }

        public static T DeserializeFromJsonBytes<T>(this byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes, jsonSerializerOptions);
        }

        public static T DeserializeFromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
        }

        public static T FromDictionary<T>(this IDictionary<string, object> dictionary)
        {
            var json = JsonSerializer.Serialize(dictionary, jsonSerializerOptions);

            return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
        }
    }
}
