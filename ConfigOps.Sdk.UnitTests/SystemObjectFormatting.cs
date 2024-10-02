using ConfigOps.Core.Serialization;
using ConfigOps.Sdk.Dto;
using Snapshooter.Xunit;

namespace ConfigOps.Sdk.UnitTests
{
    public class SystemObjectFormatting
    {
        [Fact]
        public void Validate_SystemObject_YamlFormat()
        {
            SystemObject systemObject = new SystemObject()
            {
                ApiVersion = "v1",
                Kind = "ServiceConfig",
                Metadata = new SystemObjectMetadata()
                {
                    Name = "service-a",
                    Namespace = "default",
                    Labels = new Dictionary<string, string>()
                    {
                        { "geo", "us" },
                        { "region", "westus" },
                        { "env", "prod" }
                    }
                }
            };

            systemObject
                .SerializeToYaml()
                .MatchSnapshot();
        }
    }
}