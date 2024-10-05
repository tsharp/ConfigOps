using Newtonsoft.Json;

namespace ConfigOps.Sdk.Dto
{
    internal interface ISystemObject
    {
        string ApiVersion { get; set; }

        string Kind { get; set; }
    }
}
