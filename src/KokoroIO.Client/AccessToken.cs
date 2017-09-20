using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class AccessToken
    {
        [DefaultValue(null)]
        [DataMember, JsonProperty("name")]
        public string Name { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("token")]
        public string Token { get; set; }
    }
}