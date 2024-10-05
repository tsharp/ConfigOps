//---------------------------------------------------------------
// Copyright (c) Travis Sharp.  All rights reserved.
// See LICENSE.md in the project root for license information.
//---------------------------------------------------------------

namespace ConfigOps.Sdk.Dto
{
    using ConfigOps.Sdk.Api.v1alpha1;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SystemObject : SystemObjectBase<SystemObjectMetadata>
    {
        [JsonProperty(Order = 0)]
        public override string ApiVersion { get; set; } = Constants.CRD_API_VERSION;

        [JsonProperty(Order = 1)]
        public override string Kind { get; set; } = nameof(SystemObject);
    }
}