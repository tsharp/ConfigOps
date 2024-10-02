using ConfigOps.Sdk.Dto;
using System.Runtime.Serialization;

namespace ConfigOps.Sdk.Api.v1alpha1
{
    [DataContract]
    public sealed class ServiceGroupSpec : ConfigObjectSpec
    {
        [DataMember(Order = 20)]
        public string? CloudProvider { get; set; }

        [DataMember(Order = 21)]
        public string? CloudProviderRegion { get; set; }
    }
}
