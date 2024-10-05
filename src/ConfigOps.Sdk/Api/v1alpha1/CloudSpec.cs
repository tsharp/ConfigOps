using ConfigOps.Sdk.Dto;

namespace ConfigOps.Sdk.Api.v1alpha1
{
    public sealed class CloudSpec : ConfigObjectData
    {
        public string? Provider { get; set; }
    }
}
