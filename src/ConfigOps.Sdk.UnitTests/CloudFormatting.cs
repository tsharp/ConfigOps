using ConfigOps.Sdk.Api.v1alpha1;
using ConfigOps.Sdk.Serialization;
using Snapshooter.Xunit;

namespace ConfigOps.Sdk.UnitTests
{
    public class CloudFormatting
    {
        [Fact]
        public void Validate_AzureCloud_YamlFormat()
        {
            Cloud cloud = new Cloud()
            {
                Metadata =
                {
                    Name = "cloud",
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

            cloud
                .ObjectToYaml()
                .MatchSnapshot();
        }

        [Fact]
        public void Validate_AwsCloud_YamlFormat()
        {
            Cloud cloud = new Cloud()
            {
                Metadata =
                {
                    Name = "cloud",
                    Namespace = "amazon-aws"

                },
                Data =
                {
                    Provider = "AWS",
                    ExtensionData =
                    {
                        { "subscriptionId", "12345678-1234-1234-1234-123456789012" },
                        { "tenantId", "12345678-1234-1234-1234-123456789012" },
                        { "clientId", "12345678-1234-1234-1234-123456789012" },
                        { "clientSecret", "12345678-1234-1234-1234-123456789012" }
                    }
                }
            };

            cloud
                .ObjectToYaml()
                .MatchSnapshot();
        }
    }
}
