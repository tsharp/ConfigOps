using ConfigOps.Sdk.Api.v1alpha1;
using ConfigOps.Sdk.Serialization;
using Snapshooter.Xunit;

namespace ConfigOps.Sdk.UnitTests
{
    public class ServiceGroupFormatting
    {
        [Fact]
        public void Validate_AwsServiceGroup_YamlFormat()
        {
            ServiceGroup serviceGroup = new ServiceGroup()
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

            serviceGroup
                .ObjectToYaml()
                .MatchSnapshot();
        }
    }
}
