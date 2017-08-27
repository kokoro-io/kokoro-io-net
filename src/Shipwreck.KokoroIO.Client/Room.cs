using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shipwreck.KokoroIO
{
    [Serializable]
    public class Room
    {
        [DefaultValue(null)]
        [JsonProperty("id")]
        public string Id { get; set; }

        [DefaultValue(null)]
        [JsonProperty("channel_name")]
        public string ChannelName { get; set; }

        [DefaultValue(default(RoomKind))]
        [JsonProperty("kind")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RoomKind Kind { get; set; }

        [DefaultValue(null)]
        [JsonProperty("description")]
        public string Description { get; set; }

        [DefaultValue(null)]
        [JsonProperty("membership")]
        public Membership Membership { get; set; }
    }
}