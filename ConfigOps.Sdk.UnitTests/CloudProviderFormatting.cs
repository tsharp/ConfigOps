using ConfigOps.Core.Serialization;
using ConfigOps.Sdk.Api.v1alpha1;
using Snapshooter.Xunit;

namespace ConfigOps.Sdk.UnitTests
{
    public class CloudProviderFormatting
    {
        [Fact]
        public void Validate_AzureCloudProvider_YamlFormat()
        {
            CloudProvider cloudProvider = new CloudProvider()
            {
                ApiVersion = "config.ops.kuipersys.com/v1alpha1",
                Kind = "CloudProvider",
                Metadata =
                {
                    Name = "cloud-provider",
                    Namespace = "microsoft-azure"
                },
                Spec =
                {
                    Provider = "AzureCloud",
                    Settings =
                    {
                        { "subscriptionId", "12345678-1234-1234-1234-123456789012" },
                        { "tenantId", "12345678-1234-1234-1234-123456789012" },
                        { "clientId", "12345678-1234-1234-1234-123456789012" },
                        { "clientSecret", "12345678-1234-1234-1234-123456789012" }
                    }
                }
            };

            cloudProvider
                .SerializeToYaml()
                .MatchSnapshot();
        }

        [Fact]
        public void Validate_AwsCloudProvider_YamlFormat()
        {
            CloudProvider cloudProvider = new CloudProvider()
            {
                ApiVersion = "config.ops.kuipersys.com/v1alpha1",
                Kind = "CloudProvider",
                Metadata =
                {
                    Name = "cloud-provider",
                    Namespace = "amazon-aws"

                },
                Spec =
                {
                    Provider = "AWS",
                    Settings =
                    {
                        { "subscriptionId", "12345678-1234-1234-1234-123456789012" },
                        { "tenantId", "12345678-1234-1234-1234-123456789012" },
                        { "clientId", "12345678-1234-1234-1234-123456789012" },
                        { "clientSecret", "12345678-1234-1234-1234-123456789012" }
                    }
                }
            };

            cloudProvider
                .SerializeToYaml()
                .MatchSnapshot();
        }
    }
}
