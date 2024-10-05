using ConfigOps.Sdk.Dto;
using ConfigOps.Sdk.Serialization;
using NJsonSchema.Generation;
using NJsonSchema.NewtonsoftJson.Generation;
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
                .ObjectToYaml()
                .MatchSnapshot();
        }

        [Fact]
        public void GenerateSchemaWithNSwag()
        {
            var schema = JsonSchemaGenerator.FromType(typeof(SystemObject), new NewtonsoftJsonSchemaGeneratorSettings()
            {
                SchemaType = NJsonSchema.SchemaType.OpenApi3,
                FlattenInheritanceHierarchy = true,
                UseXmlDocumentation = true
            });

            var objectSchema = schema.Definitions
                .First()
                .Value;

            objectSchema.ToJson()
                .JsonToYaml()
                .MatchSnapshot();
        }
    }
}