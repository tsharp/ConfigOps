using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ConfigOps.Sdk.Dto
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ConfigObjectMetadata : SystemObjectMetadata
    {
        [JsonProperty(Order = 100)]
        public string[]? InheritedConfigs { get; set; }
    }
}
