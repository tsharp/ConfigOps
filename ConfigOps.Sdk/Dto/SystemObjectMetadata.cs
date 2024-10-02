namespace ConfigOps.Sdk.Dto
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [DataContract]
    public sealed class SystemObjectMetadata : DtoObjectBase
    {
        [DataMember(Order = 0)]
        [JsonPropertyOrder(0)]
        public string Name { get; set; }

        [DataMember(Order = 1)]
        [JsonPropertyOrder(1)]
        public string Namespace { get; set; }

        [DataMember(Order = 2)]
        [JsonPropertyOrder(2)]
        public IDictionary<string, string>? Labels { get; set; } = new Dictionary<string, string>();

        [DataMember(Order = 2)]
        [JsonPropertyOrder(2)]
        public IDictionary<string, object>? Annotations { get; set; } = new Dictionary<string, object>();
    }
}
