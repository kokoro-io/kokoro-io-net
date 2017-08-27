using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shipwreck.KokoroIO
{
    [Serializable, DataContract]
    public class Room
    {
        [DefaultValue(null)]
        [DataMember, JsonProperty("id")]
        public string Id { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("channel_name")]
        public string ChannelName { get; set; }

        [DefaultValue(default(RoomKind))]
        [DataMember, JsonProperty("kind")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RoomKind Kind { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("description")]
        public string Description { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("membership")]
        public Membership Membership { get; set; }
    }
}