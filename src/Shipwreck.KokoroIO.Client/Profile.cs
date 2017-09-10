using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shipwreck.KokoroIO
{
    [Serializable, DataContract]
    public class Profile
    {
        [DefaultValue(null)]
        [DataMember, JsonProperty("id")]
        public string Id { get; set; }

        [DefaultValue(default(ProfileType))]
        [DataMember, JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProfileType Type { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("avatar")]
        public string Avatar { get; set; }

        [DefaultValue(false)]
        [DataMember, JsonProperty("archived")]
        public bool IsArchived { get; set; }

        [DefaultValue(0)]
        [DataMember, JsonProperty("invited_room_count")]
        public int InvitedRoomCount { get; set; }
    }
}