using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ConfigOps.Sdk.Dto
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public abstract class SystemObjectBase<TObjectMetadata> : DtoObjectBase, ISystemObject
        where TObjectMetadata : SystemObjectMetadata, new()
    {
        [JsonProperty(Order = 0)]
        public abstract string ApiVersion { get; set; }

        [JsonProperty(Order = 1)]
        public abstract string Kind { get; set; }

        [JsonProperty(Order = 2)]
        public TObjectMetadata Metadata { get; set; } = new TObjectMetadata();
    }
}
