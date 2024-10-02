using ConfigOps.Core.Serialization;
using ConfigOps.Sdk.Api.v1alpha1;
using Snapshooter.Xunit;

namespace ConfigOps.Sdk.UnitTests
{
    public class CloudProviderRegionFormatting
    {
        [Fact]
        public void Validate_AzureCloudProvider_YamlFormat()
        {
            CloudProviderRegion cloudProviderRegion = new CloudProviderRegion()
            {
                Metadata =
                {
                    Name = "uswest1",
                    Namespace = "microsoft-azure"
                },
                Spec =
                {
                    RegionName = "uswest",
                    FriendlyName = "US West",
                    RegionCode = "us-west-1",
                    CloudProvider = "microsoft-azure/cloud-config",
                    State = RegionState.Active,
                    Settings =
                    {
                        { "subscriptionId", "12345678-1234-1234-1234-123456789012" },
                        { "tenantId", "12345678-1234-1234-1234-123456789012" },
                        { "clientId", "12345678-1234-1234-1234-123456789012" },
                        { "clientSecret", "12345678-1234-1234-1234-123456789012" }
                    }
                }
            };

            cloudProviderRegion
                .SerializeToYaml()
                .MatchSnapshot();
        }
    }
}
