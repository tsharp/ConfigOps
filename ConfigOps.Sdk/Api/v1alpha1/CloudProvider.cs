using ConfigOps.Sdk.Dto;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ConfigOps.Sdk.Api.v1alpha1
{
    [DataContract]
    public sealed class CloudProvider : ConfigObject<CloudProviderSpec>
    {
        [DataMember(Order = 0)]
        [JsonPropertyOrder(0)]
        public override string ApiVersion => "config.ops.kuipersys.com/v1alpha1";

        [DataMember(Order = 10)]
        [JsonPropertyOrder(10)]
        public override string Kind => nameof(CloudProvider);
    }
}
