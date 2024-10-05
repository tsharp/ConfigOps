using ConfigOps.Sdk.Api.v1alpha1;
using ConfigOps.Sdk.Serialization;
using Snapshooter.Xunit;

namespace ConfigOps.Sdk.UnitTests
{
    public class CloudRegionFormatting
    {
        [Fact]
        public void Validate_AzureCloudRegion_YamlFormat()
        {
            CloudRegion cloudProviderRegion = new CloudRegion()
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

            cloudProviderRegion
                .ObjectToYaml()
                .MatchSnapshot();
        }
    }
}
