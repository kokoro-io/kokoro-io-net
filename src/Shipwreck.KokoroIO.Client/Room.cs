using System;
using System.ComponentModel;
using Newtonsoft.Json;

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

        // TODO: Enum?
        [DefaultValue(null)]
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [DefaultValue(null)]
        [JsonProperty("description")]
        public string Description { get; set; }

        [DefaultValue(null)]
        [JsonProperty("membership")]
        public Membership Membership { get; set; }
    }
}