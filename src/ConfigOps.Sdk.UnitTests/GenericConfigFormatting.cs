using ConfigOps.Sdk.Api.v1alpha1;
using ConfigOps.Sdk.Serialization;
using Snapshooter.Xunit;

namespace ConfigOps.Sdk.UnitTests
{
    public class GenericConfigFormatting
    {
        [Fact]
        public void GenericConfig_Serialize()
        {
            var config = new GenericConfig
            {
                Metadata =
                {
                    Name = "global-config",
                    Namespace = "global",
                    InheritedConfigs = [
                        "global/app-config-base-1"
                    ],
                },
                Data =
                {
                    ExtensionData =
                    {
                        { "setting1", "value1" },
                        { "setting2", "value2" }
                    }
                }
            };

            config
                 .ObjectToYaml()
                 .MatchSnapshot();
        }
    }
}
