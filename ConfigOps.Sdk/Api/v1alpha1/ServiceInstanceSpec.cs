using ConfigOps.Sdk.Dto;
using System.Runtime.Serialization;

namespace ConfigOps.Sdk.Api.v1alpha1
{
    [DataContract]
    public sealed class ServiceInstanceSpec : ConfigObjectSpec
    {
        [DataMember(Order = 20)]
        public string? ServiceGroup { get; set; }
    }
}