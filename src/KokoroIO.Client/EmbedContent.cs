using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class EmbedContent
    {
        [DefaultValue(null)]
        [DataMember, JsonProperty("url")]
        public string Url { get; set; }

        [DefaultValue(0)]
        [DataMember, JsonProperty("position")]
        public int Position { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("data")]
        public EmbedData Data { get; set; }
    }
}