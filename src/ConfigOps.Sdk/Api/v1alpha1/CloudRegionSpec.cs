using ConfigOps.Sdk.Dto;

namespace ConfigOps.Sdk.Api.v1alpha1
{
    public class CloudRegionSpec : ConfigObjectData
    {
        public string? CloudProvider { get; set; }

        public string? RegionName { get; set; }

        public string? FriendlyName { get; set; }

        public string? RegionCode { get; set; }

        public RegionState State { get; set; } = default;
    }
}
