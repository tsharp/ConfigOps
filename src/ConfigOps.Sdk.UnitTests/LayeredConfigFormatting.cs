using ConfigOps.Sdk.Api.v1alpha1;
using ConfigOps.Sdk.Serialization;
using Newtonsoft.Json.Linq;
using Snapshooter.Xunit;

namespace ConfigOps.Sdk.UnitTests
{
    public class LayeredConfigFormatting
    {
        [Fact]
        public void LayeredConfig_Serialize()
        {
            var cloud = Cloud();
            var cloudObject = cloud.Data.ToJsonObject();
            var cloudRegion = CloudRegion();
            var cloudRegionObject = cloudRegion.Data.ToJsonObject();
            var serviceGroup = ServiceGroup();
            var serviceGroupObject = serviceGroup.Data.ToJsonObject();
            var serviceInstance = ServiceInstance();
            var serviceInstanceObject = serviceInstance.Data.ToJsonObject();

            var root = new JObject();

            IDictionary<string, object> cloudRegions = new Dictionary<string, object>();
            IDictionary<string, object> serviceGroups = new Dictionary<string, object>();
            IDictionary<string, object> serviceInstances = new Dictionary<string, object>();
            cloudRegions.Add(cloudRegion.Metadata.Name, cloudRegionObject);
            serviceGroups.Add(serviceGroup.Metadata.Name, serviceGroupObject);
            serviceInstances.Add(serviceInstance.Metadata.Name, serviceInstanceObject);

            cloudObject.Add("regions", cloudRegions.ToJsonObject());
            serviceGroupObject.Add("instances", serviceInstances.ToJsonObject());

            root.Add("cloud", cloudObject);
            root.Add("region", cloudRegionObject);
            root.Add("services", serviceGroups.ToJsonObject());

            root
                .ObjectToYaml()
                .MatchSnapshot($"{nameof(LayeredConfigFormatting)}.{nameof(LayeredConfig_Serialize)}.yaml");

            root
                .ObjectToJson()
                .MatchSnapshot($"{nameof(LayeredConfigFormatting)}.{nameof(LayeredConfig_Serialize)}.json");
        }

        private Cloud Cloud()
        {
            return new Cloud()
            {
                Metadata =
                {
                    Name = "cloud-provider",
                    Namespace = "microsoft-azure"
                },
                Data =
                {
                    Provider = "AzureCloud",
                    ExtensionData =
                    {
                        { "subscriptionId", "12345678-1234-1234-1234-123456789012" },
                        { "tenantId", "12345678-1234-1234-1234-123456789012" },
                        { "clientId", "12345678-1234-1234-1234-123456789012" },
                        { "clientSecret", "12345678-1234-1234-1234-123456789012" }
                    }
                }
            };
        }

        private CloudRegion CloudRegion()
        {
            return new CloudRegion()
            {
                Metadata =
                {
                    Name = "uswest1",
                    Namespace = "microsoft-azure"
                },
                Data =
                {
                    RegionName = "uswest",
                    FriendlyName = "US West",
                    RegionCode = "us-west-1",
                    CloudProvider = "microsoft-azure/cloud-config",
                    State = RegionState.Active,
                    ExtensionData =
                    {
                        { "subscriptionId", "12345678-1234-1234-1234-123456789012" },
                        { "tenantId", "12345678-1234-1234-1234-123456789012" },
                        { "clientId", "12345678-1234-1234-1234-123456789012" },
                        { "clientSecret", "12345678-1234-1234-1234-123456789012" }
                    }
                }
            };
        }

        private ServiceGroup ServiceGroup()
        {
            return new ServiceGroup()
            {
                Metadata =
                {
                    Name = "service-example-uswest1",
                    Namespace = "amazon-aws"

                },
                Data =
                {
                    CloudProvider = "amazon-aws/cloud-config",
                    CloudProviderRegion = "amazon-aws/us-west-1",
                    ExtensionData =
                    {
                        { "secretStoreXyz", "abcd" }
                    }
                }
            };
        }

        private ServiceInstance ServiceInstance()
        {
            return new ServiceInstance()
            {
                Metadata =
                {
                    Name = "service-example-uswest1",
                    Namespace = "amazon-aws"

                },
                Data =
                {
                    ServiceGroup = "amazon-aws/cloud-config",
                    ExtensionData =
                    {
                        { "secretStoreXyz", "abcd" }
                    }
                }
            };
        }
    }
}
