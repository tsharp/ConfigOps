using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConfigOps.Core.Serialization
{
    internal static class JsonSerializerExtensions
    {
        public static JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions()
        {
            IgnoreReadOnlyProperties = true,
            IgnoreReadOnlyFields = true,
            IncludeFields = true,
            MaxDepth = 100,
            WriteIndented = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static byte[] SerializeToJsonBytes(this object value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value, value.GetType(), JsonSerializerOptions);
        }

        public static string SerializeToJson(this object value)
        {
            return JsonSerializer.Serialize(value, value.GetType(), JsonSerializerOptions);
        }

        public static string SerializeToJson(this object value, JsonSerializerContext context)
        {
            return JsonSerializer.Serialize(value, value.GetType(), context);
        }

        public static T DeserializeFromJsonBytes<T>(this byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes, JsonSerializerOptions);
        }

        public static T DeserializeFromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
        }

        public static T ObjectFromDictionary<T>(this IDictionary<string, object> dictionary)
        {
            var json = JsonSerializer.Serialize(dictionary, JsonSerializerOptions);

            return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
        }

        public static IDictionary<string, object> ObjectToDictionary(this object value)
        {
            var json = JsonSerializer.Serialize(value, value.GetType(), JsonSerializerOptions);

            return JsonSerializer.Deserialize<IDictionary<string, object>>(json, JsonSerializerOptions);
        }

        public static T Clone<T>(this T source)
            => source.SerializeToJson().DeserializeFromJson<T>();
    }
}
