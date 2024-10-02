using ConfigOps.Sdk.Dto;
using System.Runtime.Serialization;

namespace ConfigOps.Sdk.Api.v1alpha1
{
    public class CloudProviderRegionSpec : ConfigObjectSpec
    {
        [DataMember(Order = 0)]
        public string? CloudProvider { get; set; }

        [DataMember(Order = 10)]
        public string? RegionName { get; set; }
        [DataMember(Order = 11)]
        public string? FriendlyName { get; set; }
        [DataMember(Order = 12)]
        public string? RegionCode { get; set; }
        [DataMember(Order = 13)]
        public RegionState State { get; set; } = default;
    }
}
