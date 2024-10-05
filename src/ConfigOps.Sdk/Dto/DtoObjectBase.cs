namespace ConfigOps.Sdk.Dto
{
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
    public abstract class DtoObjectBase
    {
        [JsonExtensionData]
        public IDictionary<string, object> ExtensionData { get; init; } = new Dictionary<string, object>();
    }
}
