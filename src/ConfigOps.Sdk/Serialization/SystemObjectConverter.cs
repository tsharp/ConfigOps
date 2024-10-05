using System;
using ConfigOps.Sdk.Dto;
using Newtonsoft.Json;

namespace ConfigOps.Sdk.Serialization
{
    public class SystemObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ISystemObject).IsAssignableFrom(objectType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
