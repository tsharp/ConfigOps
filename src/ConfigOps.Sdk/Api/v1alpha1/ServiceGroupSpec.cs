using ConfigOps.Sdk.Dto;

namespace ConfigOps.Sdk.Api.v1alpha1
{
    public sealed class ServiceGroupSpec : ConfigObjectData
    {
        public string? CloudProvider { get; set; }

        public string? CloudProviderRegion { get; set; }
    }
}
