using ConfigOps.Core.Serialization;
using ConfigOps.Sdk.Api.v1alpha1;
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
                ApiVersion = "config.ops.kuipersys.com/v1alpha1",
                Metadata =
                {
                    Name = "service-example-uswest1",
                    Namespace = "amazon-aws"

                },
                Spec =
                {
                    CloudProvider = "amazon-aws/cloud-config",
                    CloudProviderRegion = "amazon-aws/us-west-1",
                    Settings =
                    {
                        { "secretStoreXyz", "abcd" }
                    }
                }
            };

            serviceGroup
                .SerializeToYaml()
                .MatchSnapshot();
        }
    }
}
