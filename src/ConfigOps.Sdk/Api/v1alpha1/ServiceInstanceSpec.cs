using ConfigOps.Sdk.Dto;

namespace ConfigOps.Sdk.Api.v1alpha1
{
    public sealed class ServiceInstanceSpec : ConfigObjectData
    {
        public string? ServiceGroup { get; set; }
    }
}