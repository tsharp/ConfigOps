//using System.Collections.Generic;
//using System.IO;
//using System.Text.Json;
//using YamlDotNet.RepresentationModel;
//using YamlDotNet.Serialization;

//namespace ConfigOps.Core.Serialization
//{
//    internal static class SerializationExtensions
//    {
//        public static string JsonToYaml(this string json)
//        {
//            JsonDocument jsonDocument = JsonDocument.Parse(json);

//            var serializer = new SerializerBuilder()
//                .WithTypeConverter(new JsonElementYamlTypeConverter())
//                .Build();

//            return serializer
//                .Serialize(jsonDocument.RootElement)
//                .Trim();
//        }

//        public static string YamlToJson(this string yaml)
//        {
//            var deserializer = new Deserializer();
//            var yamlObject = deserializer.Deserialize(yaml);

//            var serializer = new SerializerBuilder()
//                .JsonCompatible()
//                .Build();

//            return serializer
//                .Serialize(yamlObject)
//                .Trim();
//        }

//        public static string YamlToJson(this YamlNode value)
//        {
//            var serializer = new SerializerBuilder()
//                .JsonCompatible()
//                .Build();

//            return serializer
//                .Serialize(value)
//                .Trim();
//        }

//        public static IEnumerable<string> MultiYamlToJson(this string yaml)
//        {
//            var jsonDocuments = new List<string>();

//            using (var reader = new StringReader(yaml))
//            {
//                // Use YamlStream to correctly parse multiple documents
//                var yamlStream = new YamlStream();
//                yamlStream.Load(reader);

//                foreach (var document in yamlStream.Documents)
//                {
//                    jsonDocuments.Add(document.RootNode.YamlToJson());
//                }
//            }

//            return jsonDocuments.ToArray();
//        }
//    }
//}
