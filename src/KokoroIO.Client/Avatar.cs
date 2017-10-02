using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class Avatar
    {
        [DefaultValue(0)]
        [DataMember, JsonProperty("size")]
        public int Size { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("url")]
        public string Url { get; set; }

        [DefaultValue(false)]
        [DataMember, JsonProperty("is_default")]
        public bool IsDefault { get; set; }
    }
}