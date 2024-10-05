using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConfigOps.Sdk.Dto
{
    [DataContract]
    public abstract class ConfigObjectData
    {
        [JsonExtensionData]
        public IDictionary<string, object> ExtensionData { get; init; } = new Dictionary<string, object>();
    }
}
