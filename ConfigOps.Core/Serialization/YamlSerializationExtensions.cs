﻿using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace ConfigOps.Core.Serialization
{
    internal static class YamlSerializationExtensions
    {
        public static string SerializeToYaml(this object value)
        {
            var serializer = new SerializerBuilder()
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitEmptyCollections | DefaultValuesHandling.OmitNull)
                .WithTypeConverter(new JsonElementYamlTypeConverter())
                .Build();

            return serializer.Serialize(value);
        }

        public static T DeserializeFromYaml<T>(this string yaml)
        {
            var deserializer = new DeserializerBuilder()
                .WithTypeConverter(new JsonElementYamlTypeConverter())
                .Build();

            return deserializer.Deserialize<T>(yaml);
        }

        public static List<T> DeserializeMultipleFromYaml<T>(this string yaml)
        {
            var resources = new List<T>();

            using (var reader = new StringReader(yaml))
            {
                // Use YamlStream to correctly parse multiple documents
                var yamlStream = new YamlStream();
                yamlStream.Load(reader);

                foreach (var document in yamlStream.Documents)
                {
                    var result = document.RootNode
                        .SerializeToYaml()
                        .DeserializeFromYaml<IDictionary<string, object>>();

                    if (typeof(T) == typeof(IDictionary<string, object>))
                    {
                        resources.Add((T)result);
                    }
                    else
                    {
                        resources.Add(result.FromDictionary<T>());
                    }
                }
            }

            return resources;
        }
    }
}
