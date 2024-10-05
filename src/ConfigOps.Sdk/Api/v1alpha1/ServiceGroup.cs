using ConfigOps.Sdk.Dto;
using Newtonsoft.Json;

namespace ConfigOps.Sdk.Api.v1alpha1
{
    public sealed class ServiceGroup : ConfigObject<ServiceGroupSpec, ConfigObjectMetadata>
    {
        [JsonProperty(Order = 0)]
        public override string ApiVersion { get; set; } = Constants.CRD_API_VERSION;

        [JsonProperty(Order = 1)]
        public override string Kind { get; set; } = nameof(ServiceGroup);
    }
}