namespace ConfigOps.Sdk.Dto
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [DataContract]
    public abstract class DtoObjectBase
    {
        [JsonExtensionData]
        public IDictionary<string, object>? ExtensionData { get; set; } = new Dictionary<string, object>();
    }
}
