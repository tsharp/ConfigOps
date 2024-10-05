namespace ConfigOps.Sdk.Dto
{
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SystemObjectMetadata : DtoObjectBase
    {
        [DataMember(Order = 0)]
        public string Name { get; set; }

        [DataMember(Order = 1)]
        public string Namespace { get; set; }

        [DataMember(Order = 2)]
        public IDictionary<string, string>? Labels { get; init; } = new Dictionary<string, string>();

        [DataMember(Order = 3)]
        public IDictionary<string, object>? Annotations { get; init; } = new Dictionary<string, object>();

        public bool ShouldSerializeAnnotations()
        {
            return Annotations != null && Annotations.Any();
        }

        public bool ShouldSerializeLabels()
        {
            return Labels != null && Labels.Any();
        }
    }
}
