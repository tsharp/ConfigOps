//---------------------------------------------------------------
// Copyright (c) Travis Sharp.  All rights reserved.
// See LICENSE.md in the project root for license information.
//---------------------------------------------------------------

namespace ConfigOps.Sdk.Dto
{
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [DataContract]
    public class SystemObject : DtoObjectBase
    {
        [DataMember(Order = 0)]
        [JsonPropertyOrder(0)]
        public virtual string ApiVersion { get; set; }

        [DataMember(Order = 10)]
        [JsonPropertyOrder(10)]
        public virtual string Kind { get; set; }

        [DataMember(Order = 20)]
        [JsonPropertyOrder(20)]
        public SystemObjectMetadata Metadata { get; set; } = new SystemObjectMetadata();
    }
}