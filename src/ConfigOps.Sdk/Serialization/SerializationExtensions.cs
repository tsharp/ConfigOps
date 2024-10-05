using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace ConfigOps.Sdk.Serialization
{
    internal static class SerializationExtensions
    {
        public static byte[] ToBytes(this string value)
            => System.Text.Encoding.UTF8.GetBytes(value);

        public static string FromBytes(this byte[] value)
            => System.Text.Encoding.UTF8.GetString(value);

        public static string ObjectToYaml(this object obj)
            => obj.ObjectToJson().JsonToYaml();

        public static string ObjectToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, obj.GetType(), new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>
                { 
                    new StringEnumConverter()
                }
            });
        }

        public static JObject ToJsonObject(this object obj)
            => JObject.Parse(obj.ObjectToJson());

        public static string JsonToYaml(this string json)
        {
            dynamic jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());

            var serializer = new SerializerBuilder()
                .Build();

            return serializer
                .Serialize(jsonObject)
                .Trim();
        }

        public static T JsonToObject<T>(this string json)
            where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                }
            })!;
        }

        public static T YamlToObject<T>(this string yaml)
            where T: class, new()
        {
            return yaml
                .YamlToJson()
                .JsonToObject<T>();
        }

        public static string YamlToJson(this string yaml)
        {
            var deserializer = new Deserializer();

            using StringReader stringReader = new StringReader(yaml);
            var yamlObject = deserializer.Deserialize(stringReader);

            return yamlObject.ObjectToJson();
        }

        public static string YamlToJson(this YamlNode value)
        {
            var serializer = new SerializerBuilder()
                .Build();

            return serializer
                .Serialize(value)
                .YamlToJson();
        }

        public static T YamlToObject<T>(this YamlNode value)
            where T: class, new()
        {
            var json = value.YamlToJson();

            return json.JsonToObject<T>();
        }

        public static IEnumerable<string> MultiYamlToJson(this string yaml)
        {
            var jsonDocuments = new List<string>();

            using (var reader = new StringReader(yaml))
            {
                // Use YamlStream to correctly parse multiple documents
                var yamlStream = new YamlStream();
                yamlStream.Load(reader);

                foreach (var document in yamlStream.Documents)
                {
                    jsonDocuments.Add(document.RootNode.YamlToJson());
                }
            }

            return jsonDocuments.ToArray();
        }

        public static IEnumerable<T> MultiYamlToObject<T>(this string yaml)
            where T : class, new()
        {
            var objects = new List<T>();

            using (var reader = new StringReader(yaml))
            {
                // Use YamlStream to correctly parse multiple documents
                var yamlStream = new YamlStream();
                yamlStream.Load(reader);

                foreach (var document in yamlStream.Documents)
                {
                    objects.Add(document.RootNode.YamlToObject<T>());
                }
            }

            return objects.ToArray();
        }
    }
}
