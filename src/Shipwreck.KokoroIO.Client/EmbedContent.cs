using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Shipwreck.KokoroIO
{
    [Serializable, DataContract]
    public class EmbedContent
    {
        [DefaultValue(null)]
        [DataMember, JsonProperty("url")]
        public string Url { get; set; }

        // TODO: temporary removed because apidoc error.
        //[DefaultValue(null)]
        //[DataMember, JsonProperty("data")]
        //public string Data { get; set; }
    }
}