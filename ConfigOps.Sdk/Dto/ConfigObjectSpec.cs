using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConfigOps.Sdk.Dto
{
    [DataContract]
    public abstract class ConfigObjectSpec
    {
        [DataMember(Order = 100000)]
        public List<string> InheritedConfigs { get; set; } = new List<string>();

        [DataMember(Order = 100001)]
        public IDictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();
    }
}
