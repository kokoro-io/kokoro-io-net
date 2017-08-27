using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Shipwreck.KokoroIO
{
    [Serializable]
    public class Profile
    {
        [DefaultValue(0)]
        [JsonProperty("id")]
        public int Id { get; set; }

        // TODO: Enum?
        [DefaultValue(null)]
        [JsonProperty("type")]
        public string Type { get; set; }

        [DefaultValue(null)]
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [DefaultValue(null)]
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [DefaultValue(null)]
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [DefaultValue(false)]
        [JsonProperty("archived")]
        public bool IsArchived { get; set; }

        [DefaultValue(0)]
        [JsonProperty("invited_room_count")]
        public int InvitedRoomCount { get; set; }
    }
}