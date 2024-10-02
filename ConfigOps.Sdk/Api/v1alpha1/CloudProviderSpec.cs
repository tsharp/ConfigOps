using ConfigOps.Sdk.Dto;
using System.Runtime.Serialization;

namespace ConfigOps.Sdk.Api.v1alpha1
{
    [DataContract]
    public sealed class CloudProviderSpec : ConfigObjectSpec
    {
        [DataMember(Order = 0)]
        public string? Provider { get; set; }
    }
}
